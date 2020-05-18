using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionMarshaller.Model
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
        public int Quantity { get; set; }

        public int Quality { get; set; } = 0;
    }
}
