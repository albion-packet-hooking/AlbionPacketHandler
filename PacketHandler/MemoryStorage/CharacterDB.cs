using AlbionMarshaller.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlbionMarshaller.MemoryStorage
{
    public class PlayerEventArgs : EventArgs
    {
        public Player Player { get; set; }
        public PlayerEventArgs(Player player)
        {
            Player = player;
        }
    }
    public class CharacterDB
    {
        public event System.EventHandler<PlayerEventArgs> PlayerAdded;
        public event System.EventHandler<PlayerEventArgs> PlayerRemoved;

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

        private Dictionary<String, Player> characters = new Dictionary<String, Player>();
        private CharacterDB()
        {
            AddPlayer(Self);
        }

        private static readonly Lazy<CharacterDB> lazy = new Lazy<CharacterDB>(() => new CharacterDB());
        public static CharacterDB Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public Player Self { get; set; } = new Player() { Id = -1000, Name = "OWN_GOD_DAMN_PLAYER" };

        public bool PlayerExists(String characterName)
        {
            return characters.ContainsKey(characterName);
        }

        public Player FindByID(int ID)
        {
            KeyValuePair<String, Player> player = this.characters.FirstOrDefault(p => p.Value.Id == ID);
            if(player.Key != null)
            {
                return player.Value;
            }
            return null;
        }

        public Player FindByName(string name)
        {
            Player player = null;
            if(characters.TryGetValue(name, out player))
            {
                return player;
            }
            return null;
        }

        public void AddPlayer(Player player)
        {
            if(!PlayerExists(player.Name))
            {
                PlayerAdded?.Invoke(this, new PlayerEventArgs(player));
                characters.Add(player.Name, player);
            }
        }

        // Don't actually remove the player, just inform others they have left
        public void RemovePlayer(Player player)
        {
            if (player != null && !PlayerExists(player.Name))
            {
                PlayerRemoved?.Invoke(this, new PlayerEventArgs(player));
            }
        }

        public CharacterType ClassifyCharacter(Player player)
        {
            foreach (ItemSlot slot in classifiers.Keys)
            {
                Dictionary<String, CharacterType> slotClassifiers = classifiers[slot];
                if (player.Gear.ContainsKey(slot))
                {
                    JObject item = player.Gear[slot];
                    string subtype = item["Subtype"].ToString();
                    if (slotClassifiers.ContainsKey(subtype))
                    {
                        return slotClassifiers[subtype];
                    }
                }
            }

            return CharacterType.Unknown;
        }
    }
}
