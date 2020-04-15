using log4net;
using AlbionProcessor.MemoryStorage;
using AlbionProcessor.Model;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AlbionProcessor
{
    public class NewCharacterHandler : AlbionEvent
    {
        [EventHandler(EventCodes.NewCharacter)]
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
            foreach (int itemID in equipment)
            {
                if (itemID > 0)
                {
                    JObject item = ItemDB.Instance.FindItem(itemID);
                    if(!player.Gear.Contains(item))
                    {
                        player.Gear.Add(item);
                    }
                    equipStr += item["UniqueName"] + " | ";
                }
            }

            log.Info($"{playerName} = {equipStr}");
        }
    }
}
