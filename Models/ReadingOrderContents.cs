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

        public bool deleteConnector((Guid, Guid) orign, (Guid, Guid) destination)
        {
            for (int i = 0; i < Connectors.Count; i++)
            {
                if (Connectors[i].origin == orign && Connectors[i].destination == destination)
                {
                    Connectors.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public bool NodeExistsAtPosition(Guid x, Guid y)
        {
            foreach (Node node in this.Nodes)
            {
                if(node.GetX() == x && node.GetY() == y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ConnectorExistsBetween((Guid, Guid) origin, (Guid, Guid) destination)
        {
            foreach (Connector connector in Connectors)
            {
                if (connector.origin == origin && connector.destination == destination)
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteNode(Node node)
        {
            return this.Nodes.Remove(node);
        }

        /// <summary>
        /// Deletes all nodes and connectors in a given row
        /// </summary>
        /// <param name="rowId"></param>
        public void DeleteAllContentsInRow(Guid rowId)
        {
            this.Nodes.RemoveAll(node => node.GetY() == rowId);
            this.Connectors.RemoveAll(connector => connector.origin.Item2 == rowId || connector.destination.Item2 == rowId);
        }

        /// <summary>
        /// Deletes all nodes and connectors in a given column
        /// </summary>
        /// <param name="colId"></param>
        public void DeleteAllContentsInColumn(Guid colId)
        {
            this.Nodes.RemoveAll(node => node.GetX() == colId);
            this.Connectors.RemoveAll(connector => connector.origin.Item1 == colId || connector.destination.Item1 == colId);
        }
    }
}
