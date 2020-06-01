using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AlbionMarshaller.MemoryStorage
{
    public class ItemDB
    {
        private Dictionary<int, JObject> itemDictionary = new Dictionary<int, JObject>();
        private ItemDB()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AlbionMarshaller.Resources.items.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                JArray itemArray = JArray.Parse(reader.ReadToEnd());
                foreach (JObject item in itemArray)
                {
                    int id = int.Parse(item["Index"].ToString());
                    itemDictionary.Add(id, item);
                }
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
