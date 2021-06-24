using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

    public enum ItemLevel
    {
        Journeymans = 3,
        Adepts = 4,
        Experts = 5,
        Masters = 6,
        Grandmasters = 7,
        Elders = 8
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

    public enum FactionFlag
    {
        None = 0,
        Martlock = 1,
        Lymhurst = 2,
        Bridgewatch = 3,
        FortSterling = 4,
        Thetford = 5,
        Caerleon = 6,
        Red = 255
    }

    public class Player : INotifyPropertyChanged
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
        public Dictionary<ItemSlot, Item> Gear { get; set; } = new Dictionary<ItemSlot, Item>();

        private float[] _coordinates;
        public float[] Coordinates
        {
            get
            {
                return _coordinates;
            }
            set
            {
                _coordinates = value;
                OnPropertyChanged();
            }
        }

        public bool InParty
        {
            get; set;
        }

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
                            Item item = Gear[slot];
                            string subtype = item.Subcategory;
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

        public FactionFlag Flag
        {
            get; set;
        }

        private String _text = null;

        public event PropertyChangedEventHandler PropertyChanged;

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

        private ItemSlot[] _importantItems = new ItemSlot[] { ItemSlot.MainHand, ItemSlot.OffHand, ItemSlot.Shoes, ItemSlot.Body, ItemSlot.Helmet, ItemSlot.Cape, ItemSlot.Mount };
        public String EquipmentString
        {
            get
            {
                if (Gear.Count > 0)
                {
                    StringBuilder gearBuilder = new StringBuilder();
                    foreach(ItemSlot slotType in _importantItems)
                    {
                        if(Gear.ContainsKey(slotType))
                        {
                            Item item = Gear[slotType];
                            string itemName = item.UniqueName;
                            String localizationName = item.LocalizedName;
                            if (localizationName != null)
                            {
                                int i = localizationName.IndexOf(" ") + 1;
                                localizationName = localizationName.Substring(i);

                                string tier = itemName.Substring(0, itemName.IndexOf("_"));
                                if (!tier.StartsWith("T"))
                                {
                                    tier = "";
                                }

                                if (itemName.Contains("@"))
                                {
                                    string quality = itemName.Split('@')[1];
                                    itemName = $"{tier}.{quality} {localizationName}";
                                }
                                else
                                {
                                    itemName = $"{tier} {localizationName}";
                                }
                            }
                            gearBuilder.Append($"{itemName} | ");
                        }
                    }

                    return gearBuilder.ToString();
                }
                return null;
            }
        }

        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
