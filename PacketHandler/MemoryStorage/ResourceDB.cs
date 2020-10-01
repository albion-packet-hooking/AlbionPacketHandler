using AlbionMarshaller.Model;
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
    public class ResourceEventArgs : EventArgs
    {
        public Resource Resource { get; set; }
        public ResourceEventArgs(Resource resource)
        {
            Resource = resource;
        }
    }
    public class ResourceRemoveEventArgs : EventArgs
    {
        public int ResourceId { get; set; }
        public ResourceRemoveEventArgs(int resourceId)
        {
            ResourceId = resourceId;
        }
    }
    public class ResourceChangedEventArgs : EventArgs
    {
        public Resource Resource { get; set; }
        public string PropertyName { get; set; }
        public ResourceChangedEventArgs(Resource resource, string propertyName)
        {
            Resource = resource;
            PropertyName = propertyName;
        }
    }

    public class ResourceDB
    {
        public class ResourceType
        {
            public string Type { get; set; }
            public string Name { get; set; }
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<int, ResourceType> resourceDictionary = new Dictionary<int, ResourceType>();
        private Dictionary<string, ResourceType> resourceDictionaryByName = new Dictionary<string, ResourceType>();

        private Dictionary<int, Resource> objectIdToResourceMap = new Dictionary<int, Resource>();

        public HashSet<string> ResourceTypes { get; } = new HashSet<string>();
        
        public event System.EventHandler<ResourceEventArgs> ResourceAdded;
        public event System.EventHandler<ResourceRemoveEventArgs> ResourceRemoved;
        public event System.EventHandler<ResourceChangedEventArgs> ResourceChanged;

        private ResourceDB()
        {
            log.Info("Loading resources into memory");
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AlbionMarshaller.Resources.resources.xml";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                XDocument mobDoc = XDocument.Parse(reader.ReadToEnd());

                int index = 0;
                foreach (XElement topResource in mobDoc.Root.Element("Harvestables").Elements("Harvestable"))
                {
                    string name = topResource.Attribute("name").Value;
                    string resource = null;
                    if (topResource.Attribute("resource") != null)
                    {
                        resource = topResource.Attribute("resource").Value;
                        ResourceTypes.Add(resource);
                    }
                    ResourceType resType = new ResourceType() { Type = resource, Name = name };
                    resourceDictionary.Add(index++, resType);
                    resourceDictionaryByName.Add(name, resType);
                    //foreach (XElement subResource in topResource.Elements("ResourceTier"))
                    //{
                    //    int tier = int.Parse(subResource.Attribute("value").Value);
                    //}
                }
            }
            log.Info("Finished loading resources into memory");
        }

        private static readonly Lazy<ResourceDB> lazy = new Lazy<ResourceDB>(() => new ResourceDB());
        public static ResourceDB Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public Resource FindOrCreateResource(int objectId, int resourceId, int tier)
        {
            if (objectIdToResourceMap.ContainsKey(objectId))
            {
                return objectIdToResourceMap[objectId];
            }

            if (resourceDictionary.ContainsKey(resourceId))
            {
                ResourceType resType = resourceDictionary[resourceId];
                Resource newResource = new Resource() { ResourceId = resourceId, Name = resType.Name, Type = resType.Type, Tier = tier };
                objectIdToResourceMap.Add(objectId, newResource);

                if (ResourceAdded != null)
                {
                    ResourceEventArgs m = new ResourceEventArgs(newResource);
                    foreach (System.EventHandler<ResourceEventArgs> e in ResourceAdded?.GetInvocationList())
                    {
                        e.BeginInvoke(this, m, e.EndInvoke, null);
                    }
                }

                return newResource;
            }

            return null;
        }

        public Resource FindResource(int objectId)
        {
            return objectIdToResourceMap.ContainsKey(objectId) ? objectIdToResourceMap[objectId] : null;
        }

        public ResourceType FindResourceTypeByName(string name)
        {
            if(resourceDictionaryByName.ContainsKey(name))
            {
                return resourceDictionaryByName[name];
            }
            return null;
        }

        public void RemoveResource(int objectId)
        {
            if (objectIdToResourceMap.ContainsKey(objectId))
            {
                objectIdToResourceMap.Remove(objectId);

                if (ResourceRemoved != null)
                {
                    ResourceRemoveEventArgs m = new ResourceRemoveEventArgs(objectId);
                    foreach (System.EventHandler<ResourceRemoveEventArgs> e in ResourceRemoved?.GetInvocationList())
                    {
                        e.BeginInvoke(this, m, e.EndInvoke, null);
                    }
                }
            }
        }

        public void Clear()
        {
            if (ResourceRemoved != null)
            {
                ResourceRemoveEventArgs m = new ResourceRemoveEventArgs(-1);
                foreach (System.EventHandler<ResourceRemoveEventArgs> e in ResourceRemoved?.GetInvocationList())
                {
                    e.BeginInvoke(this, m, e.EndInvoke, null);
                }
            }

            objectIdToResourceMap.Clear();
        }

        public void NotifyChange(Resource resource, string propertyName)
        {
            if (ResourceChanged != null)
            {
                ResourceChangedEventArgs m = new ResourceChangedEventArgs(resource, propertyName);
                foreach (System.EventHandler<ResourceChangedEventArgs> e in ResourceChanged?.GetInvocationList())
                {
                    e.BeginInvoke(this, m, e.EndInvoke, null);
                }
            }
        }
    }
}
