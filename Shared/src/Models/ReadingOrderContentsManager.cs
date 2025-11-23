namespace ROGraph.Shared.Models
{
    public class ReadingOrderContentsManager
    {
        private List<Node> Nodes { get; set; }

        private List<Connector> Connectors { get; set; }

        public ReadingOrderContentsManager()
        {
            this.Nodes = new List<Node>();
            this.Connectors = new List<Connector>();
        }

        public ReadingOrderContentsManager(List<Node> nodes, List<Connector> connectors)
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

        public List<Node> GetNodes()
        {
            return this.Nodes;
        }

        public void AddNode(Node node)
        {
            this.Nodes.Add(node);
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

        /// <summary>
        /// Gets all connectors that originate from or point to position
        /// </summary>
        /// <param name="posiion"></param>
        /// <returns></returns>
        public IEnumerable<Connector> GetConnectors((Guid, Guid) posiion)
        {
            return this.Connectors.Where(c => c.origin == posiion || c.destination == posiion);
        }

        public void AddConnector(Connector connector)
        {
            this.Connectors.Add(connector);
        }

        public bool DeleteConnector((Guid, Guid) orign, (Guid, Guid) destination)
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
                if(node.X == x && node.Y == y)
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
            (Guid, Guid) nodePosition = node.GetPosition();
            this.Connectors.RemoveAll(connector => connector.origin == nodePosition || connector.destination == nodePosition);
            return this.Nodes.Remove(node);
        }

        /// <summary>
        /// Deletes all nodes and connectors in a given row
        /// </summary>
        /// <param name="rowId"></param>
        public void DeleteAllContentsInRow(Guid rowId)
        {
            this.Nodes.RemoveAll(node => node.Y == rowId);
            this.Connectors.RemoveAll(connector => connector.origin.Item2 == rowId || connector.destination.Item2 == rowId);
        }

        /// <summary>
        /// Deletes all nodes and connectors in a given column
        /// </summary>
        /// <param name="colId"></param>
        public void DeleteAllContentsInColumn(Guid colId)
        {
            this.Nodes.RemoveAll(node => node.Y == colId);
            this.Connectors.RemoveAll(connector => connector.origin.Item1 == colId || connector.destination.Item1 == colId);
        }
    }
}
