using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ROGraph.Models
{
    public class ReadingOrderContents
    {
        public List<Node> Nodes { get; set; }

        public List<Connector> Connectors { get; set; }

        public ReadingOrderContents()
        {
            this.Nodes = [];
            this.Connectors = [];
        }

        public ReadingOrderContents(List<Node> nodes, List<Connector> connectors)
        {
            this.Nodes = nodes;
            this.Connectors = connectors;
        }
    }
}
