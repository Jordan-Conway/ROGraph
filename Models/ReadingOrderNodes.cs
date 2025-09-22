using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ROGraph.Models
{
    public class ReadingOrderNodes
    {
        public List<List<Node?>> Nodes { get; set; }

        public List<List<Line>> Connectors { get; set; }

        public ReadingOrderNodes()
        {
            this.Nodes = [];
            this.Connectors = [];
        }

        public ReadingOrderNodes(List<List<Node?>> nodes, List<List<Line>> connectors)
        {
            this.Nodes = nodes;
            this.Connectors = connectors;
        }

        public int Count()
        {
            int count = 0;

            foreach(List<Node> ln in this.Nodes)
            {
                count += ln.Count();
            }

            return count;
        }
    }
}
