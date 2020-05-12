using AlbionMarshaller.MemoryStorage;
using AlbionMarshaller.Model;
using System.IO;

namespace AlbionProcessor
{
    public class AlbionProcessor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AlbionProcessor()
        {
            PacketHandler.Instance.Initialize();
            LootDB.Instance.LootAddedToPlayer += PlayerLootAdded;
        }

        private void PlayerLootAdded(object sender, PlayerLootEventArgs plea)
        {
            Loot item = plea.Loot;
            Player player = plea.Player;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "aolootlog", PacketHandler.Instance.LogTimer + ".csv");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string logMessage = $"[{item.PickupTime.ToString()}] {player.Name} has looted {item.Quantity}x {item.ItemName} from {item.BodyName}";
            string csvMessage = $"{item.PickupTime.ToString()};{player.Name};{item.ItemName};{item.Quantity};{item.BodyName}";

            log.Info(logMessage);
            using (StreamWriter streamWriter = File.AppendText(path))
            {
                streamWriter.WriteLine(csvMessage);
            }
        }
    }
}
