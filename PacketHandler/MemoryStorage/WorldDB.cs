using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AlbionMarshaller.MemoryStorage
{
    public class WorldDB
    {
        private static readonly Lazy<WorldDB> lazy = new Lazy<WorldDB>(() => new WorldDB());
        public static WorldDB Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private Dictionary<string, string> _nameToDisplay = new Dictionary<string, string>();
        private WorldDB()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AlbionMarshaller.Resources.world.xml";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                XDocument worldDoc = XDocument.Parse(reader.ReadToEnd());

                foreach (XElement loc in worldDoc.Root.Element("clusters").Descendants("cluster"))
                {
                    string id = loc.Attribute("id").Value;
                    string displayName = loc.Attribute("displayname").Value;
                    _nameToDisplay.Add(id, displayName);
                }
            }
        }

        public string GetDisplayName(string id)
        {
            if(_nameToDisplay.ContainsKey(id))
            {
                return _nameToDisplay[id];
            }

            return null;
        }
    }
}
