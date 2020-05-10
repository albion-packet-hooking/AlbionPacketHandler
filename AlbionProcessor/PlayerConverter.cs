using AlbionMarshaller.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AlbionProcessor
{
    public class PlayerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            ObservableCollection<Player> players = (ObservableCollection<Player>)value;
            List<Player> playersWithLoot = new List<Player>();
            foreach (Player player in players)
            {
                if(player.Loots.Count == 0)
                {
                    continue;
                }
                playersWithLoot.Add(player);
            }

            JArray playerObj = JArray.FromObject(playersWithLoot);
            playerObj.WriteTo(writer);
        }
    }
}
