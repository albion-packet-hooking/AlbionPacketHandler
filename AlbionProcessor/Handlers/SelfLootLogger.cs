using log4net;
using AlbionProcessor.MemoryStorage;
using AlbionProcessor.Model;
using System;
using System.Collections.Generic;

namespace AlbionProcessor.Handlers
{
    public class SelfAlbionProcessor : AlbionEvent
    {
        [EventHandler(EventCodes.InventoryPutItem)]
        public static void ProcessInventoryPutItem(Dictionary<byte, object> parameters, ILog log)
        {
            log.Info(parameters);
            int objectID = int.Parse(parameters[0].ToString());

            Loot item = LootDB.Instance.FindLoot(objectID);
            if(item == null)
            {
                return;
            }

            if(String.IsNullOrEmpty(item.BodyName))
            {
                return;
            }

            item.LooterName = CharacterDB.Instance.Self.Name;

            LootDB.Instance.AddLootToPlayer(item, CharacterDB.Instance.Self);

            LootDB.Instance.RemoveItem(objectID);
        }

        [OperationHandler(OperationCodes.InventoryMoveItem)]
        public static void InventoryMoveItem(Dictionary<byte, object> parameters, ILog log)
        {
            if(!parameters.ContainsKey(1))
            {
                return;
            }
            int position = parameters.ContainsKey(0) ? int.Parse(parameters[0].ToString()) : 0;
            Guid containerGuid = new Guid((byte[])parameters[1]);

            Container container = LootDB.Instance.FindByGUID(containerGuid);
            if (container == null)
            {
                log.Error($"Missing container: {containerGuid}");
                return;
            }

            if (container.Type != ContainerType.Unknown && container.Type != ContainerType.Player)
            {
                Loot loot = container.Loot[position];
                if (loot != null)
                {
                    loot.LooterName = CharacterDB.Instance.Self.Name;

                    LootDB.Instance.AddLootToPlayer(loot, CharacterDB.Instance.Self);

                    LootDB.Instance.RemoveItem(loot.ObjectID);
                    container.Loot[position] = null;
                }
            }
        }

        [OperationHandler(OperationCodes.Join)]
        public static void ResponseJoin(Dictionary<byte, object> parameters, ILog log)
        {
            Guid ownGuid = new Guid((byte[])parameters[1]);
            string name = parameters[2].ToString();
            string guild = parameters.ContainsKey(50) ? parameters[50].ToString() : "";
            string alliance = parameters.ContainsKey(68) ? parameters[68].ToString() : "";

            Player player = CharacterDB.Instance.FindByID(-1000);
            player.Name = name;
            player.Guild = guild;
            player.Alliance = alliance;
            player.Guid = ownGuid;

            // Serialize this player out
        }
    }
}
