using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AlbionMarshaller.Model
{
    public enum CharacterType
    {
        Healer,
        RangedDPS,
        MagicDPS,
        Tank,
        Support,
        MeleeDPS,
        WoodGather,
        StoneGather,
        SkinGather,
        FiberGather,
        OreGather,
        Fisher,
        Unknown
    }

    public enum ItemSlot
    {
        MainHand = 0,
        OffHand = 1,
        Helmet = 2,
        Body = 3,
        Shoes = 4,
        Bag = 5,
        Cape = 6,
        Mount = 7,
        Food,
        Potion
    }

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
        public Dictionary<ItemSlot, JObject> Gear { get; set; } = new Dictionary<ItemSlot, JObject>();

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

        private void DetermineCharacterType()
        {

        }
    }
}
