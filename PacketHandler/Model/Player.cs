using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

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
        private Dictionary<ItemSlot, Dictionary<String, CharacterType>> classifiers = new Dictionary<ItemSlot, Dictionary<String, CharacterType>>
        {
            {
                ItemSlot.MainHand,
                    new Dictionary<String, CharacterType>
                        {
                            { "naturestaff", CharacterType.Healer },
                            { "holystaff", CharacterType.Healer },
                            { "hammer", CharacterType.Tank },
                            { "mace", CharacterType.Tank },
                            { "quarterstaff", CharacterType.Tank },
                            { "bow", CharacterType.RangedDPS },
                            { "crossbow", CharacterType.RangedDPS },
                            { "sword", CharacterType.MeleeDPS },
                            { "axe", CharacterType.MeleeDPS },
                            { "dagger", CharacterType.MeleeDPS },
                            { "spear", CharacterType.MeleeDPS },
                            { "firestaff", CharacterType.MagicDPS },
                            { "froststaff", CharacterType.MagicDPS },
                            { "cursestaff", CharacterType.Support },
                            { "arcanestaff", CharacterType.Support },
                            { "sickle", CharacterType.FiberGather },
                            { "pickaxe", CharacterType.OreGather },
                            { "woodaxe", CharacterType.WoodGather },
                            { "stonehammer", CharacterType.StoneGather },
                            { "fishing", CharacterType.Fisher },
                        }
            }
        };

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

        private CharacterType type = CharacterType.Unknown;
        public CharacterType CharacterType
        {
            get
            {
                if(type == CharacterType.Unknown)
                {
                    foreach (ItemSlot slot in classifiers.Keys)
                    {
                        Dictionary<String, CharacterType> slotClassifiers = classifiers[slot];
                        if (Gear.ContainsKey(slot))
                        {
                            JObject item = Gear[slot];
                            string subtype = item["Subtype"].ToString();
                            if (slotClassifiers.ContainsKey(subtype))
                            {
                                type = slotClassifiers[subtype];
                                break;
                            }
                        }
                    }
                }
                return type;
            }
        }

        private String _text = null;
        public String Text
        {
            get
            {
                if (_text == null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (!String.IsNullOrEmpty(Alliance))
                    {
                        sb.Append($"[{Alliance}]");
                    }
                    if (!String.IsNullOrEmpty(Guild))
                    {
                        sb.Append($"{Guild}|");
                    }
                    sb.Append($"{Name} ({CharacterType})");

                    _text = sb.ToString();
                }

                return _text;
            }
        }

        public String EquipmentString
        {
            get
            {
                if (Gear.Count > 0)
                {
                    StringBuilder gearBuilder = new StringBuilder();
                    foreach (KeyValuePair<ItemSlot, JObject> gearItem in Gear)
                    {
                        JObject item = gearItem.Value;
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
                        gearBuilder.Append($"{itemName} | ");
                    }

                    return gearBuilder.ToString();
                }
                return null;
            }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
