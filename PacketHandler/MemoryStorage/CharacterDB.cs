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
        public event System.EventHandler ZoneChanged;

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

        public string CurrentLocation { get; set; } = "-1";

        public bool PlayerExists(String characterName)
        {
            return characters.ContainsKey(characterName);
        }

        public bool InCity()
        {
            if(CurrentLocation == "-1")
            {
                return false;
            }
            string location = WorldDB.Instance.GetDisplayName(CurrentLocation);
            if(location.StartsWith("Martlock") ||
                location.StartsWith("Fort Sterling") ||
                location.StartsWith("Bridgewatch") ||
                location.StartsWith("Thetford") ||
                location.StartsWith("Lymhurst"))
            {
                return true;
            }
            return false;
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
            if (PlayerAdded != null)
            {
                PlayerEventArgs pea = new PlayerEventArgs(player);
                foreach (System.EventHandler<PlayerEventArgs> e in PlayerAdded?.GetInvocationList())
                {
                    e.BeginInvoke(this, pea, e.EndInvoke, null);
                }
            }
            if (!PlayerExists(player.Name))
            {
                characters.Add(player.Name, player);
            }
        }

        // Don't actually remove the player, just inform others they have left
        public void RemovePlayer(Player player)
        {
            if (player != null && PlayerExists(player.Name) && PlayerRemoved != null)
            {
                PlayerEventArgs pea = new PlayerEventArgs(player);
                foreach (System.EventHandler<PlayerEventArgs> e in PlayerRemoved?.GetInvocationList())
                {
                    e.BeginInvoke(this, pea, e.EndInvoke, null);
                }
            }
        }

        public void ChangeLocation(string newLocationID)
        {
            if (newLocationID != CurrentLocation)
            {
                CurrentLocation = newLocationID;

                if (ZoneChanged != null)
                {
                    foreach (System.EventHandler e in ZoneChanged.GetInvocationList())
                    {
                        e.BeginInvoke(this, null, e.EndInvoke, null);
                    }
                }
            }
        }

        public void Clear()
        {
            if (PlayerRemoved != null)
            {
                PlayerEventArgs pea = new PlayerEventArgs(new Player() { Id = -1 });
                foreach (System.EventHandler<PlayerEventArgs> e in PlayerRemoved?.GetInvocationList())
                {
                    e.BeginInvoke(this, pea, e.EndInvoke, null);
                }
            }
        }
    }
}
