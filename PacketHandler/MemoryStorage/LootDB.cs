using AlbionMarshaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbionMarshaller.MemoryStorage
{
    public class LootEventArgs : EventArgs
    {
        public Loot Loot { get; set; }
        public LootEventArgs(Loot loot)
        {
            Loot = loot;
        }
    }
    public class PlayerLootEventArgs : EventArgs
    {
        public Loot Loot { get; set; }
        public Player Player { get; set; }
        public PlayerLootEventArgs(Loot loot, Player player)
        {
            Loot = loot;
            Player = player;
        }
    }

    public class LootDB
    {
        public event EventHandler<LootEventArgs> LootAdded;
        public event EventHandler<PlayerLootEventArgs> LootAddedToPlayer;

        private Dictionary<int, Loot> lootTable = new Dictionary<int, Loot>();
        private Dictionary<string, Container> containers = new Dictionary<string, Container>();
        private Dictionary<Guid, Container> guidToContainers = new Dictionary<Guid, Container>();

        private static readonly Lazy<LootDB> lazy = new Lazy<LootDB>(() => new LootDB());
        public static LootDB Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public void AddLoot(Loot loot)
        {
            if(lootTable.ContainsKey(loot.ObjectID))
            {
                // Remove the old loot and register the new one
                lootTable.Remove(loot.ObjectID);
            }

            if (LootAdded != null)
            {
                LootEventArgs eventArgs = new LootEventArgs(loot);
                foreach (EventHandler<LootEventArgs> e in LootAdded?.GetInvocationList())
                {
                    Task.Run(() => e.Invoke(this, eventArgs));
                }
            }

            lootTable.Add(loot.ObjectID, loot);
        }

        public Loot FindLoot(int objectID)
        {
            return lootTable.ContainsKey(objectID) ? lootTable[objectID] : null;
        }

        public void RemoveItem(int objectID)
        {
            if(lootTable.ContainsKey(objectID))
            {
                lootTable.Remove(objectID);
            }
        }

        public void AttachContainer(Container container)
        {
            if (!containers.ContainsKey(container.ID))
            {
                containers.Add(container.ID, container);
            }
            if(container.GUID != Guid.Empty && !guidToContainers.ContainsKey(container.GUID))
            {
                guidToContainers.Add(container.GUID, container);
            }
        }

        public void DetachContainer(string containerId)
        {
            if (containers.ContainsKey(containerId))
            {
                containers.Remove(containerId);
            }
        }

        public void DetachContainer(Guid containerGuid)
        {
            Container container = FindByGUID(containerGuid);
            if(container != null)
            {
                containers.Remove(container.ID);
            }
        }

        public Container FindByID(string containerId)
        {
            if (containerId != null && containers.ContainsKey(containerId))
            {
                return containers[containerId];
            }
            return null;
        }

        public Container FindByGUID(Guid containerGuid)
        {
            if(containerGuid == Guid.Empty)
            {
                return null;
            }
            // Try to find by GUID
            KeyValuePair<string, Container> containerPair = containers.FirstOrDefault(c => containerGuid.Equals(c.Value.GUID));
            if (containerPair.Key != null)
            {
                return containerPair.Value;
            }

            return null;
        }

        public Container FindByID(string containerId, Guid containerGuid)
        {
            if(containerGuid != Guid.Empty && guidToContainers.ContainsKey(containerGuid))
            {
                return guidToContainers[containerGuid];
            }
            Container container = FindByID(containerId);
            return container;
        }

        public void AddLootToPlayer(Loot item, Player player)
        {
            player.Loots.Add(item);

            if (LootAddedToPlayer != null)
            {
                PlayerLootEventArgs pea = new PlayerLootEventArgs(item, player);
                foreach (EventHandler<PlayerLootEventArgs> e in LootAddedToPlayer?.GetInvocationList())
                {
                    Task.Run(() => e.Invoke(this, pea));
                }
            }
        }

        public void AddLootToPlayer(Loot item, string playerName)
        {
            Player player = CharacterDB.Instance.FindByName(playerName);
            if (player == null)
            {
                player = new Player() { Name = playerName, Loots = new List<Loot>() };
                CharacterDB.Instance.AddPlayer(player);
            }
            item.LooterName = playerName;

            player.Loots.Add(item);

            if (LootAddedToPlayer != null)
            {
                PlayerLootEventArgs pea = new PlayerLootEventArgs(item, player);
                foreach (EventHandler<PlayerLootEventArgs> e in LootAddedToPlayer?.GetInvocationList())
                {
                    Task.Run(() => e.Invoke(this, pea));
                }
            }
        }
    }
}
