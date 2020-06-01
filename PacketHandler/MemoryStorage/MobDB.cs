using AlbionMarshaller.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace AlbionMarshaller.MemoryStorage
{
    public class MobEventArgs : EventArgs
    {
        public Mob Mob { get; set; }
        public MobEventArgs(Mob mob)
        {
            Mob = mob;
        }
    }
    public class MobRemoveEventArgs : EventArgs
    {
        public int MobId { get; set; }
        public MobRemoveEventArgs(int mobId)
        {
            MobId = mobId;
        }
    }
    public class MobChangedEventArgs : EventArgs
    {
        public Mob Mob { get; set; }
        public string PropertyName { get; set; }
        public MobChangedEventArgs(Mob mob, string propertyName)
        {
            Mob = mob;
            PropertyName = propertyName;
        }
    }
    public class MobDB
    {
        public struct MobDef
        {
            public string Type { get; set; }
            public string UniqueName { get; set; }
            public string Name { get; set; }

            public ResourceDB.ResourceType HarvestableType { get; set; }
            public int HarvestableTier { get; set; }
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<int, MobDef> mobDictionary = new Dictionary<int, MobDef>();
        private Dictionary<string, HashSet<string>> harvestableToMob = new Dictionary<string, HashSet<string>>();

        private Dictionary<int, Mob> objectToMobMap = new Dictionary<int, Mob>();

        public event EventHandler<MobEventArgs> MobAdded;
        public event EventHandler<MobRemoveEventArgs> MobRemoved;
        public event EventHandler<MobChangedEventArgs> MobChanged;

        private MobDB()
        {
            log.Info("Loading Mobs into memory");

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AlbionMarshaller.Resources.mobs.xml";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                XDocument mobDoc = XDocument.Parse(reader.ReadToEnd());

                int index = 0;
                foreach (XElement mob in mobDoc.Root.Elements())
                {
                    String mobName = mob.Attribute("uniquename").Value;
                    String localeName = $"@MOB_{mobName}";
                    if (mob.Attribute("namelocatag") != null)
                    {
                        localeName = mob.Attribute("namelocatag").Value;
                    }
                    MobDef def = new MobDef() { UniqueName = mobName, Name = Localization.Instance.Find(localeName) };

                    XElement lootNode = mob.Element("Loot");
                    if (lootNode != null)
                    {
                        XElement harvestable = lootNode.Element("Harvestable");
                        if (harvestable != null)
                        {
                            int tier = int.Parse(harvestable.Attribute("tier").Value);
                            String type = harvestable.Attribute("type").Value;
                            def.HarvestableType = ResourceDB.Instance.FindResourceTypeByName(type);
                            def.HarvestableTier = tier;
                        }
                    }

                    if (def.HarvestableType != null && def.HarvestableType.Type != null && def.HarvestableTier > 1)
                    {
                        if (harvestableToMob.ContainsKey(def.HarvestableType.Type))
                        {
                            harvestableToMob[def.HarvestableType.Type].Add(def.Name);
                        }
                        else
                        {
                            harvestableToMob.Add(def.HarvestableType.Type, new HashSet<string>() { def.Name });
                        }
                    }
                    mobDictionary.Add(index++, def);
                }
            }
            log.Info("Finished loading Mobs into memory");
        }

        private static readonly Lazy<MobDB> lazy = new Lazy<MobDB>(() => new MobDB());
        public static MobDB Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public Mob FindOrCreateMob(int objectId, int mobType)
        {
            if (objectToMobMap.ContainsKey(objectId))
            {
                return objectToMobMap[objectId];
            }

            if (mobDictionary.ContainsKey(mobType))
            {
                MobDef resType = mobDictionary[mobType];
                Mob newMob = new Mob() { MobId = objectId, Name = resType.Name, HarvestableTier = resType.HarvestableTier, HarvestableType = resType.HarvestableType == null ? null : resType.HarvestableType.Type };
                objectToMobMap.Add(objectId, newMob);
                MobAdded?.Invoke(this, new MobEventArgs(newMob));
                return newMob;
            }

            return null;
        }

        public Mob FindMob(int objectId)
        {
            return objectToMobMap.ContainsKey(objectId) ? objectToMobMap[objectId] : null;
        }

        public void RemoveMob(int objectId)
        {
            if (objectToMobMap.ContainsKey(objectId))
            {
                objectToMobMap.Remove(objectId);
                MobRemoved?.Invoke(this, new MobRemoveEventArgs(objectId));
            }
        }

        public void Clear()
        {
            MobRemoved?.Invoke(this, new MobRemoveEventArgs(-1));
            objectToMobMap.Clear();
        }

        public void NotifyChange(Mob mob, string propertyName)
        {
            MobChanged?.Invoke(this, new MobChangedEventArgs(mob, propertyName));
        }

        public HashSet<string> GetMobsByResourceType(string resourceType)
        {
            if(harvestableToMob.ContainsKey(resourceType))
            {
                return harvestableToMob[resourceType];
            }
            else
            {
                return new HashSet<string>();
            }
        }
    }
}
