using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using AlbionMarshaller;
using AlbionMarshaller.Model;
using AlbionMarshaller.MemoryStorage;

namespace AlbionProcessor
{
    public class LootEventHandler : BaseEvent
    {
        [AlbionMarshaller.EventHandler(EventCodes.PartyLootItems)]
        public static void PartyLootItems(Dictionary<byte, object> parameters, ILog log)
        {
            if (!(parameters.ContainsKey(1) &&
                     parameters.ContainsKey(2) &&
                     parameters.ContainsKey(9) &&
                     parameters.ContainsKey(10)))
            {
                return;
            }

            string containerId = parameters[0].ToString();
            Container container = LootDB.Instance.FindByID(containerId);
            int[] itemIDs = Convert(parameters[1]);
            int[] itemTypeIDs = Convert(parameters[2]);
            int[] quantities = Convert(parameters[9]);
            string[] names = (string[])parameters[10];

            for (int index = 0; index < itemIDs.Length; index++)
            {
                int itemID = itemIDs[index];
                int itemTypeID = itemTypeIDs[index];
                int quantity = quantities[index];
                string playerName = names[index];

                Loot loot = LootDB.Instance.FindLoot(itemID);
                if (loot == null)
                {
                    Item item = ItemDB.Instance.FindItem(itemTypeID);
                    string itemName = item.UniqueName;
                    if (item.LocalizationName != null)
                    {
                        itemName += " - " + item.LocalizedName;
                    }

                    loot = new Loot()
                    {
                        ObjectID = itemID,
                        ItemRefId = itemTypeID,
                        LooterName = playerName,
                        Quantity = quantity,
                        LocalPickupTime = DateTime.Now.ToLocalTime(),
                        UtcPickupTime = DateTime.UtcNow,
                        ItemName = item.UniqueName,
                        LongName = itemName,
                        BodyName = container.Owner
                    };

                    LootDB.Instance.AddLoot(loot);
                }
                else
                {
                    loot.LooterName = playerName;
                    loot.BodyName = container.Owner;
                }
                container.Loot.Add(loot);
                //LootDB.Instance.AddLootToPlayer(loot, playerName);
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.PartyLootItemsRemoved)]
        public static void ProcessPartyLootItemsRemoved(Dictionary<byte, object> parameters, ILog log)
        {
            Container container = LootDB.Instance.FindByID(parameters[0].ToString());

            HashSet<int> items = new HashSet<int>(Convert(parameters[1]));
            foreach (int itemID in items)
            {
                Loot item = LootDB.Instance.FindLoot(itemID);
                if (item != null)
                {
                    LootDB.Instance.AddLootToPlayer(item, item.LooterName);
                    int index = container.Loot.FindIndex(i => i != null && i.ObjectID == itemID);
                    if (index > 0)
                    {
                        container.Loot[index] = null;
                    }
                    LootDB.Instance.RemoveItem(itemID);
                }
                else
                {
                    log.Info($"Missing item {itemID} {container.ID}");
                }
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.OtherGrabbedLoot)]
        public static void ProcessOtherGrabbedLoot(Dictionary<byte, object> parameters, ILog log)
        {
            if (!(parameters.ContainsKey(2) &&
                     parameters.ContainsKey(5) &&
                     parameters.ContainsKey(4) &&
                     parameters.ContainsKey(1)))
            {
                return;
            }

            string looter = parameters[2].ToString();
            string quantity = parameters[5].ToString();
            int itemId = int.Parse(parameters[4].ToString());
            Item item = ItemDB.Instance.FindItem(itemId);
            string itemName = item.UniqueName;
            if (item.LocalizationName != null)
            {
                itemName += " - " + item.LocalizedName;
            }

            string deadPlayer = parameters[1].ToString();

            Loot loot = new Loot
            {
                ItemRefId = itemId,
                ItemName = item.UniqueName,
                LongName = itemName,
                Quantity = int.Parse(quantity.ToString()),
                LocalPickupTime = DateTime.UtcNow.ToLocalTime(),
                UtcPickupTime = DateTime.UtcNow,
                BodyName = deadPlayer,
                LooterName = looter
            };

            if (!loot.ItemName.Contains("TRASH"))
            {
                LootDB.Instance.AddLootToPlayer(loot, looter);
            }
        }
    }
}
