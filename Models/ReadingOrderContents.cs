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

        public Node? GetNode(Guid id)
        {
            foreach (Node node in Nodes)
            {
                if(node.Id == id)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Replaces a node with the same id as the parameter node
        /// </summary>
        /// <param name="node"></param>
        /// <returns>
        /// True is successfully replaced. Returns false in all other cases
        /// </returns>
        public bool ReplaceNode(Node node)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if(Nodes[i].Id == node.Id)
                {
                    Nodes.RemoveAt(i);
                    Nodes.Add(node);
                    return true;
                }
            }

            return false;
        }
    }
}
