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
            int playerId = int.Parse(parameters[0].ToString());
            string playerName = parameters[1].ToString();
            Guid guid = parameters.ContainsKey(7) ? new Guid((byte[])parameters[7]) : new Guid();
            string guildName = parameters.ContainsKey(8) ? parameters[8].ToString() : "";
            string alliance = parameters.ContainsKey(43) ? parameters[43].ToString() : "";

            Player player = CharacterDB.Instance.FindByName(playerName);
            if (player == null)
            {
                player = new Player() { Name = playerName, Guild = guildName, Alliance = alliance, Id = playerId, Loots = new List<Loot>(), Guid = guid };
                CharacterDB.Instance.AddPlayer(player);
            }
            else
            {
                player.Alliance = alliance;
                player.Guild = guildName;
                player.Id = playerId;
            }

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

            log.Info($"{playerName}(CharType={CharacterDB.Instance.ClassifyCharacter(player)}) = {equipStr}");
        }

        [AlbionMarshaller.EventHandler(EventCodes.Leave)]
        public static void OnLeave(Dictionary<byte, object> parameters, ILog log)
        {
            int characterID = int.Parse(parameters[0].ToString());
            Player player = CharacterDB.Instance.FindByID(characterID);
            CharacterDB.Instance.RemovePlayer(player);
        }
    }
}
