using Newtonsoft.Json;
using System;

namespace AlbionMarshaller.Model
{
    public class Loot
    {
        [JsonIgnore]
        public int ObjectID { get; set; }

        [JsonIgnore]
        public int ItemRefId { get; set; }

        [JsonProperty]
        public DateTime PickupTime { get; set; }

        [JsonProperty]
        public string BodyName { get; set; }

        [JsonProperty]
        public string ItemName { get; set; }
        [JsonProperty]
        public string LongName { get; set; }
        [JsonProperty]
        public int Quantity { get; set; }

        [JsonIgnore]
        public string LooterName { get; set; }

        public string GetLine()
        {
            return String.Join(", ", PickupTime.ToString("HH:mm:ss"), LooterName, Quantity, ItemName, BodyName)+"\\n";
        }

        public override string ToString()
        {
            return String.Join(", ", PickupTime.ToString("HH:mm:ss"), LooterName, Quantity, LongName, BodyName);
        }
    }
}
