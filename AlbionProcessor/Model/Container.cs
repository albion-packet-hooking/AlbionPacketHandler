using System;
using System.Collections.Generic;

namespace AlbionProcessor.Model
{
    public enum ContainerType
    {
        Unknown = 0,
        Monster = 1,
        Chest,
        Player
    }
    public class Container
    {
        public string ID { get; set; }
        public Guid GUID { get; set; }

        public string Owner { get; set; }
        
        public ContainerType Type { get; set; }

        public List<Loot> Loot { get; set; } = new List<Loot>();

        public override string ToString()
        {
            return $"GUID:{GUID} | ID:{ID} | Owner: {Owner} | Type: {Type}";
        }
    }
}
