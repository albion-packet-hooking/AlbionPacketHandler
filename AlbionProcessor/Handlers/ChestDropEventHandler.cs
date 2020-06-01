using AlbionMarshaller;
using AlbionMarshaller.MemoryStorage;
using AlbionMarshaller.Model;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlbionProcessor
{
    public class ChestDropEventHandler : BaseEvent
    {
        [AlbionMarshaller.EventHandler(EventCodes.DetachItemContainer)]
        public static void DetachItemContainer(Dictionary<byte, object> parameters, ILog log)
        {
            Guid containerGuid = new Guid((byte[])parameters[0]);
            log.Info($"Detaching container {containerGuid}");
            LootDB.Instance.DetachContainer(containerGuid);
        }

        [AlbionMarshaller.EventHandler(EventCodes.AttachItemContainer)]
        public static void AttachItemContainer(Dictionary<byte, object> parameters, ILog log)
        {
            string containerId = parameters[0].ToString();
            Guid containerGuid = new Guid((byte[])parameters[1]);
            Container container = LootDB.Instance.FindByID(containerId, containerGuid);

            // Make a new container
            if(container == null)
            {
                log.Info($"Attaching new container ID:{containerId} GUID:{containerGuid}");
                container = new Container();
                container.ID = containerId;
                container.GUID = containerGuid;

                LootDB.Instance.AttachContainer(container);
            }
            else
            {
                log.Info($"Updating old container Old-ID:{container.ID}->{containerId} OldGUID:{container.GUID}->{containerGuid}");
                container.ID = containerId;
                container.GUID = containerGuid;
            }

            int[] itemIDs = Convert(parameters[3]);
            if (container.Loot.Count < itemIDs.Length)
            {
                // Grow the list
                container.Loot.AddRange(Enumerable.Repeat<Loot>(null, itemIDs.Length - container.Loot.Count));
            }

            for (int index = 0; index < itemIDs.Length; index++)
            {
                int itemID = itemIDs[index];
                Loot loot = LootDB.Instance.FindLoot(int.Parse(itemID.ToString()));
                if (loot != null && !container.Loot.Contains(loot))
                {
                    if(String.IsNullOrEmpty(loot.BodyName))
                    {
                        loot.BodyName = container.Owner;
                    }
                    log.Info($"Adding loot {loot} to container {container}");
                    container.Loot.Insert(index, loot);
                }
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.UpdateLootChest)]
        public static void UpdateLootChest(Dictionary<byte, object> parameters, ILog log)
        {
            string containerID = parameters[0].ToString();
            Container container = LootDB.Instance.FindByID(containerID);

            if (container == null)
            {
                container = new Container();
                container.ID = containerID;
                container.Type = ContainerType.Chest;
                LootDB.Instance.AttachContainer(container);
                log.Info($"Attaching new container {containerID}");
            }
            else
            {
                container.ID = containerID;
                container.Type = ContainerType.Chest;
                log.Info($"Updating old container {container}");
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.NewLootChest)]
        public static void NewLootChest(Dictionary<byte, object> parameters, ILog log)
        {
            string containerID = parameters[0].ToString();
            string containerName = parameters[3].ToString();
            Container container = LootDB.Instance.FindByID(containerID);

            if (container == null)
            {
                container = new Container();
                container.ID = containerID;
                container.Type = ContainerType.Chest;
                container.Owner = containerName;
                LootDB.Instance.AttachContainer(container);
                log.Info($"Attaching new container {containerID}");
            }
            else
            {
                container.ID = containerID;
                container.Owner = containerName;
                container.Type = ContainerType.Chest;
                log.Info($"Updating old container {container}");
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.NewLoot)]
        public static void NewLoot(Dictionary<byte, object> parameters, ILog log)
        {
            string containerID = parameters[0].ToString();
            // 2 = mob id?
            string bodyName = (string)parameters[3];

            Container container = new Container();
            container.ID = containerID;
            container.Owner = bodyName;

            if (bodyName.StartsWith("@MOB"))
            {
                container.Type = ContainerType.Monster;
            }
            else
            {
                container.Type = ContainerType.Player;
            }

            Container oldContainer = LootDB.Instance.FindByID(containerID);

            if(oldContainer != null)
            {
                log.Debug($"Same container key found {container}");

                oldContainer.Type = container.Type;
                oldContainer.Owner = bodyName;
            }
            else
            { 
                LootDB.Instance.AttachContainer(container);
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.NewSimpleItem)]
        [AlbionMarshaller.EventHandler(EventCodes.NewEquipmentItem)]
        public static void NewItem(Dictionary<byte, object> parameters, ILog log)
        {
            if (!(parameters.ContainsKey(0) &&
                     parameters.ContainsKey(1)))
            {
                return;
            }

            int objectID = int.Parse(parameters[0].ToString());
            int itemID = int.Parse(parameters[1].ToString());
            int quantity = int.Parse(parameters[2].ToString());

            JObject item = ItemDB.Instance.FindItem(itemID);
            string itemName = item["UniqueName"].ToString();
            if (item.ContainsKey("LocalizedNames") && item["LocalizedNames"] != null)
            {
                JObject localizedNames = (JObject)item["LocalizedNames"];
                if (localizedNames.ContainsKey("EN-US"))
                {
                    itemName += " - " + localizedNames["EN-US"].ToString();
                }
            }

            LootDB.Instance.AddLoot(
                new Loot()
                {
                    ObjectID = objectID,
                    ItemRefId = itemID,
                    ItemName = item["UniqueName"].ToString(),
                    LongName = itemName,
                    Quantity = quantity,
                    LocalPickupTime = DateTime.Now.ToLocalTime(),
                    UtcPickupTime = DateTime.UtcNow
                }
            );
        }
    }
}
