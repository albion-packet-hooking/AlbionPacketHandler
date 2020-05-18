using AlbionMarshaller;
using AlbionMarshaller.MemoryStorage;
using AlbionMarshaller.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionProcessor.Handlers
{
    public class ResourceHandler : BaseEvent
    {
        [AlbionMarshaller.EventHandler(EventCodes.MobChangeState)]
        public static void HandleMobChangeState(Dictionary<byte, object> parameters, ILog log)
        {
            int mobId = int.Parse(parameters[0].ToString());
            int quality = int.Parse(parameters[1].ToString());
            Mob mob = MobDB.Instance.FindMob(mobId);
            if(mob != null)
            {
                mob.Quality = quality;
                log.Info($"New Mob quality object_id={mobId} name={mob.Name} quality={quality}");
                MobDB.Instance.NotifyChange(mob, "quality");
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.HarvestableChangeState)]
        public static void HandleHarvestableChangeState(Dictionary<byte, object> parameters, ILog log)
        {
            int resourceId = int.Parse(parameters[0].ToString());
            Resource resource = ResourceDB.Instance.FindResource(resourceId);
            if(resource != null)
            {
                if (parameters.ContainsKey(1))
                {
                    resource.Quantity = int.Parse(parameters[1].ToString());
                    log.Info($"Updated resource quantity object_id={resourceId} name={resource.Name} quantity={resource.Quantity}");
                    ResourceDB.Instance.NotifyChange(resource, "quantity");
                }
                if (parameters.ContainsKey(2))
                {
                    resource.Quality = int.Parse(parameters[2].ToString());
                    log.Info($"Updated resource quality object_id={resourceId} name={resource.Name} quality={resource.Quality}");
                    ResourceDB.Instance.NotifyChange(resource, "quality");
                }
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.NewMob)]
        public static void HandleNewMob(Dictionary<byte, object> parameters, ILog log)
        {
            int mobId = int.Parse(parameters[0].ToString());
            int mobType = int.Parse(parameters[1].ToString());
            Mob mob = MobDB.Instance.FindOrCreateMob(mobId, mobType);
            log.Info($"New Mob object_id={mobId} mob_name={mob.Name}");
        }

        [AlbionMarshaller.EventHandler(EventCodes.NewSimpleHarvestableObjectList)]
        public static void HandleNewHarvestables(Dictionary<byte, object> parameters, ILog log)
        {
            int[] objectIds = Convert(parameters[0]);
            int[] resourceTypes = Convert(parameters[1]);
            int[] resourceTier = Convert(parameters[2]);
            int[] quantity = Convert(parameters[4]);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < resourceTier.Length; i++)
            {
                int objectId = objectIds[i];
                int resourceType = resourceTypes[i];
                Resource resource = ResourceDB.Instance.FindOrCreateResource(objectId, resourceType, resourceTier[i]);
                resource.Quantity = quantity[i];
                if (resource != null)
                {
                    sb.Append($"id = {objectIds[i]}, resourceTier = {resourceTier[i]}, type = {resource.Type}, quantity = {quantity[i]}| ");
                }
                else
                {
                    sb.Append($"id = {objectIds[i]}, NULL RESOURCE | ");
                }
            }
            log.Info(sb.ToString());
        }

        [AlbionMarshaller.EventHandler(EventCodes.NewHarvestableObject)]
        public static void HandleNewHarvestable(Dictionary<byte, object> parameters, ILog log)
        {
            int objectId = int.Parse(parameters[0].ToString());
            int resourceType = int.Parse(parameters[5].ToString());
            int resourceTier = int.Parse(parameters[7].ToString());

            int quantity = -1;
            if (parameters.ContainsKey(10))
            {
                quantity = int.Parse(parameters[10].ToString());
            }

            int quality = int.Parse(parameters[11].ToString());
            Resource resource = ResourceDB.Instance.FindOrCreateResource(objectId, resourceType, resourceTier);
            if (resource != null)
            {
                log.Info($"New resource object_id={objectId} qualtity={quality} tier={resourceTier} type={resource.Type} quantity={quantity}");
            }
            else
            {
                log.Debug($"Null resource object_id={objectId}");
            }
        }
    }
}
