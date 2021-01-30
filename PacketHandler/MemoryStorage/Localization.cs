using AlbionMarshaller.Extractor;
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
    class Localization
    {
        private static readonly Lazy<Localization> lazy = new Lazy<Localization>(() => new Localization());
        public static Localization Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private Dictionary<string, string> _nameToTranslation = new Dictionary<string, string>();
        private Localization()
        {
            XDocument localizationDoc = ResourceLoader.LoadResource("localization");
            foreach (XElement loc in localizationDoc.Root.Descendants("tu"))
            {
                string name = loc.Attribute("tuid").Value;
                string enUS = loc.Elements("tuv").Where(x => x.Attribute(XNamespace.Xml + "lang").Value == "EN-US").Select(x => x.Value).FirstOrDefault();
                if (!_nameToTranslation.ContainsKey(name))
                {
                    _nameToTranslation.Add(name, enUS);
                }
            }
        }

        public string Find(string uniqueName)
        {
            if(_nameToTranslation.ContainsKey(uniqueName))
            {
                return _nameToTranslation[uniqueName];
            }
            else
            {
                return null;
            }
        }
    }
}
