using log4net;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using AlbionMarshaller;
using AlbionMarshaller.MemoryStorage;
using AlbionMarshaller.Model;

namespace AlbionProcessor
{
    public class CharacterHandler : BaseEvent
    {
        [AlbionMarshaller.EventHandler(EventCodes.NewCharacter)]
        public static void HandleNewCharacter(Dictionary<byte, object> parameters, ILog log)
        {
            //if(CharacterDB.Instance.InCity())
            //{
            //    return;
            //}
            int playerId = int.Parse(parameters[0].ToString());
            string playerName = parameters[1].ToString();
            Guid guid = parameters.ContainsKey(7) ? new Guid((byte[])parameters[7]) : new Guid();
            string guildName = parameters.ContainsKey(8) ? parameters[8].ToString() : "";
            string alliance = parameters.ContainsKey(43) ? parameters[43].ToString() : "";
            int flag = parameters.ContainsKey(45) ? int.Parse(parameters[45].ToString()) : 0;

            Player player = CharacterDB.Instance.FindByName(playerName);
            if (player == null)
            {
                player = new Player() { Name = playerName, Guild = guildName, Alliance = alliance, Id = playerId, Loots = new List<Loot>(), Guid = guid };
            }
            else
            {
                player.Alliance = alliance;
                player.Guild = guildName;
                player.Id = playerId;
            }

            player.Flag = (FactionFlag)flag;

            string equipStr = "";
            int[] equipment = Convert(parameters[33]);
            for (int i = 0; i < equipment.Length; i++)
            {
                int itemID = equipment[i];
                if (itemID > 0)
                {
                    JObject item = ItemDB.Instance.FindItem(itemID);
                    ItemSlot slot = (ItemSlot)i;
                    if(player.Gear.ContainsKey((ItemSlot)i))
                    {
                        player.Gear[slot] = item;
                    }
                    else
                    {
                        player.Gear.Add(slot, item);
                    }

                    string itemName = item["UniqueName"].ToString();
                    JObject localizedNames = (JObject)item["LocalizedNames"];
                    if (localizedNames.ContainsKey("EN-US"))
                    {
                        if (itemName.Contains("@"))
                        {
                            string quality = itemName.Split('@')[1];
                            itemName = $"{localizedNames["EN-US"].ToString()}.{quality}";
                        }
                        else
                        {
                            itemName = localizedNames["EN-US"].ToString();
                        }
                    }
                    equipStr += itemName + " | ";
                }
            }
            
            CharacterDB.Instance.AddPlayer(player);      
        }
        
        [AlbionMarshaller.EventHandler(EventCodes.Leave)]
        public static void OnLeave(Dictionary<byte, object> parameters, ILog log)
        {
            int objectId = int.Parse(parameters[0].ToString());
            Player player = CharacterDB.Instance.FindByID(objectId);
            if (player != null)
            {
                CharacterDB.Instance.RemovePlayer(player);
                return;
            }

            Mob mob = MobDB.Instance.FindMob(objectId);
            if(mob != null)
            {
                MobDB.Instance.RemoveMob(objectId);
                return;
            }

            Resource resource = ResourceDB.Instance.FindResource(objectId);
            if(resource != null)
            {
                ResourceDB.Instance.RemoveResource(objectId);
                return;
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.JoinFinished)]
        public static void OnJoinFinished(Dictionary<byte, object> parameters, ILog log)
        {
            CharacterDB.Instance.Clear();
            MobDB.Instance.Clear();
            ResourceDB.Instance.Clear();
        }

        [OperationHandler(OperationCodes.Join, OperationType.Response)]
        public static void OnJoin(Dictionary<byte, object> parameters, ILog log)
        {
            if (parameters.ContainsKey(8))
            {
                log.Debug($"New Location {parameters[8]}");
                CharacterDB.Instance.ChangeLocation(parameters[8].ToString());
            }
        }

        [OperationHandler(OperationCodes.ChangeCluster, OperationType.Response)]
        public static void OnClusterChange(Dictionary<byte, object> parameters, ILog log)
        {
            if (parameters.ContainsKey(0))
            {
                log.Debug($"New Location {parameters[0]}");
                CharacterDB.Instance.ChangeLocation(parameters[0].ToString());
            }
        }
    }
}
