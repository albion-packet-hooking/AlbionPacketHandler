using AlbionMarshaller;
using AlbionMarshaller.MemoryStorage;
using AlbionMarshaller.Model;
using System.IO;
using System.Threading;

namespace AlbionProcessor
{
    public class AlbionProcessor
    {
        private static object _lock = new object();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AlbionProcessor()
        {
            PhotonPacketHandler.Instance.Initialize();
            LootDB.Instance.LootAddedToPlayer += PlayerLootAdded;
        }

        public void Shutdown()
        {
            PhotonPacketHandler.Instance.Shutdown();
        }

        private void PlayerLootAdded(object sender, PlayerLootEventArgs plea)
        {
            Loot item = plea.Loot;
            Player player = plea.Player;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "aolootlog", PhotonPacketHandler.Instance.LogTimer + ".csv");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string logMessage = $"[{item.LocalPickupTime.ToString()}] {player.Name} has looted {item.Quantity}x {item.ItemName} from {item.BodyName}";

            string alliance = "";
            if(player.Alliance != null)
            {
                alliance = player.Alliance;
            }

            string guild = "";
            if (player.Guild != null)
            {
                guild = player.Guild;
            }
            string quality = "0";
            if(item.ItemName.Contains("@"))
            {
                quality = item.ItemName.Split('@')[1];
            }
            string csvMessage = $"{item.ItemRefId},{item.UtcPickupTime.ToString()},{alliance},{guild},{player.Name},{item.ItemName},{item.LongName},{quality},{item.Quantity},{item.BodyName}";

            log.Info(logMessage);
            lock (_lock)
            {
                using (StreamWriter streamWriter = File.AppendText(path))
                {
                    streamWriter.WriteLine(csvMessage);
                }
            }
        }
    }
}
