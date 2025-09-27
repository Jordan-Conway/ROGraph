using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ROGraph.Models
{
    public class Connector
    {
        public (Guid, Guid) origin { get; set; }
        public (Guid, Guid) destination { get; set; }

        public Connector((Guid, Guid) origin, (Guid, Guid) destination)
        {
            this.origin = origin;
            this.destination = destination;
        }
    }
}
