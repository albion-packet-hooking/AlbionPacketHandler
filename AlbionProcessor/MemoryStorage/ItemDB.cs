using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace AlbionProcessor.MemoryStorage
{
    public class ItemDB
    {
        private Dictionary<int, JObject> itemDictionary = new Dictionary<int, JObject>();
        private ItemDB()
        {
            JArray itemArray = JArray.Parse(File.ReadAllText(@"Resources\items.json"));
            foreach (JObject item in itemArray)
            {
                int id = int.Parse(item["Index"].ToString());
                itemDictionary.Add(id, item);
            }
        }

        private static readonly Lazy<ItemDB> lazy = new Lazy<ItemDB>(() => new ItemDB());
        public static ItemDB Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public JObject FindItem(int id)
        {
            return itemDictionary.ContainsKey(id) ? itemDictionary[id] : null;
        }
    }
}
