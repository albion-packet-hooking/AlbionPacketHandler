﻿using AlbionProcessor.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlbionProcessor.MemoryStorage
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
    }
}
