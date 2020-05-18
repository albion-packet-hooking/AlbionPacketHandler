using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionMarshaller.Model
{
    public class Mob
    {
        public int MobId { get; set; }
        public string UniqueName { get; set; }
        public string Name { get; set; }
        public int Quality { get; set; } = 0;
        public string HarvestableType { get; set; }
        public int HarvestableTier { get; set; }
    }
}
