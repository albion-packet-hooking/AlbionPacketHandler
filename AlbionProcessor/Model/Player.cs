using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AlbionProcessor.Model
{
    public class Player
    {
        public Guid Guid { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Guild { get; set; }

        [JsonProperty]
        public string Alliance { get; set; }

        [JsonProperty]
        public List<Loot> Loots { get; set; } = new List<Loot>();

        [JsonProperty]
        public List<JObject> Gear { get; set; } = new List<JObject>();

        public String Text
        {
            get
            {
                if (!String.IsNullOrEmpty(Alliance))
                {
                    return $"[{Alliance}]{Guild} | {Name}";
                }

                return Name;
            }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
