using AlbionMarshaller.Extractor;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AlbionMarshaller.MemoryStorage
{
    public class WorldDB
    {
        public class WorldObj
        {
            public string ID { get; set; }
            public string DisplayName { get; set; }
            public string WorldType { get; set; }
            public string FileName { get; set; }
            public string TimeRegion { get; set; }
            public int Tier { get; set; }
        }

        public long WorldTime { get; set; } = -1;

        private static readonly Lazy<WorldDB> lazy = new Lazy<WorldDB>(() => new WorldDB());
        public static WorldDB Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private Dictionary<string, WorldObj> _nameToDisplay = new Dictionary<string, WorldObj>();
        private WorldDB()
        {
            XDocument worldDoc = ResourceLoader.LoadResource("world");

            int i = 0;
            foreach (XElement loc in worldDoc.Root.Element("clusters").Descendants("cluster"))
            {
                string id = loc.Attribute("id").Value;
                string displayName = loc.Attribute("displayname").Value;
                string worldType = loc.Attribute("type").Value;
                string fileName = loc.Attribute("file").Value;
                string timeregion = loc.Attribute("timeregion").Value;
                int tier = int.Parse(Regex.Match(fileName, "_T(\\d)_").Groups[1].ToString());
                WorldObj worldObj = new WorldObj() { ID = id, DisplayName = displayName, WorldType = worldType, FileName = fileName, Tier = tier, TimeRegion = timeregion };
                    
                _nameToDisplay.Add(id, worldObj);
                i++;
            }
        }        

        public string GetDisplayName(string id)
        {
            if(_nameToDisplay.ContainsKey(id))
            {
                return _nameToDisplay[id].DisplayName;
            }

            return null;
        }

        public string GetWorldType(string id)
        {
            if (_nameToDisplay.ContainsKey(id))
            {
                return _nameToDisplay[id].WorldType;
            }

            return null;
        }

        public WorldObj GetWorldObj(string id)
        {
            if (_nameToDisplay.ContainsKey(id))
            {
                return _nameToDisplay[id];
            }

            return null;
        }
    }
}
