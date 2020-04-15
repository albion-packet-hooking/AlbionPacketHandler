using log4net;
using AlbionProcessor.MemoryStorage;
using AlbionProcessor.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AlbionProcessor
{
    public class LootEventHandler : AlbionEvent
    {
        [EventHandler(EventCodes.PartyLootItems)]
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
                    JObject item = ItemDB.Instance.FindItem(itemTypeID);
                    string itemName = item["UniqueName"].ToString();
                    if (item.ContainsKey("LocalizedNames"))
                    {
                        JObject localizedNames = (JObject)item["LocalizedNames"];
                        if (localizedNames.ContainsKey("EN-US"))
                        {
                            itemName += " - " + localizedNames["EN-US"].ToString();
                        }
                    }

                    loot = new Loot()
                    {
                        ObjectID = itemID,
                        ItemRefId = itemTypeID,
                        LooterName = playerName,
                        Quantity = quantity,
                        PickupTime = DateTime.Now.ToLocalTime(),
                        ItemName = item["UniqueName"].ToString(),
                        LongName = itemName,
                        BodyName = container.Owner
                    };
                }

                container.Loot.Add(loot);
                LootDB.Instance.AddLootToPlayer(loot, playerName);
            }
        }

        [EventHandler(EventCodes.OtherGrabbedLoot)]
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
            JObject item = ItemDB.Instance.FindItem(itemId);
            string itemName = item["UniqueName"].ToString();
            if (item.ContainsKey("LocalizedNames"))
            {
                JObject localizedNames = (JObject)item["LocalizedNames"];
                if (localizedNames.ContainsKey("EN-US"))
                {
                    itemName += " - " + localizedNames["EN-US"].ToString();
                }
            }

            string deadPlayer = parameters[1].ToString();

            Loot loot = new Loot
            {
                ItemRefId = itemId,
                ItemName = item["UniqueName"].ToString(),
                LongName = itemName,
                Quantity = int.Parse(quantity.ToString()),
                PickupTime = DateTime.UtcNow.ToLocalTime(),
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
