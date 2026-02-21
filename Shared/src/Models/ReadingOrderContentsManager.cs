namespace ROGraph.Shared.Models
{
    public class ReadingOrderContentsManager
    {
        private List<Node> Nodes { get; set; }

        private List<Connector> Connectors { get; set; }

        public ReadingOrderContentsManager()
        {
            Nodes = [];
            Connectors = [];
        }

        public ReadingOrderContentsManager(List<Node> nodes, List<Connector> connectors)
        {
            Nodes = nodes;
            Connectors = connectors;
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
            return Nodes;
        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
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

        public IEnumerable<Connector> GetConnectors()
        {
            return Connectors;
        }

        /// <summary>
        /// Gets all connectors that originate from or point to position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public IEnumerable<Connector> GetConnectorsBetweenPositions((Guid, Guid) position)
        {
            return Connectors.Where(c => c.Origin == position || c.Destination == position);
        }

        public void AddConnector(Connector connector)
        {
            Connectors.Add(connector);
        }

        public bool DeleteConnector((Guid, Guid) origin, (Guid, Guid) destination)
        {
            for (var i = 0; i < Connectors.Count; i++)
            {
                if (Connectors[i].Origin != origin || Connectors[i].Destination != destination)
                {
                    continue;
                }
                
                Connectors.RemoveAt(i);
                return true;
            }

            return false;
        }

        public bool NodeExistsAtPosition(Guid x, Guid y)
        {
            return Nodes.Any(node => node.X == x && node.Y == y);
        }

        public bool ConnectorExistsBetween((Guid, Guid) origin, (Guid, Guid) destination)
        {
            foreach (Connector connector in Connectors)
            {
                if (connector.Origin == origin && connector.Destination == destination)
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteNode(Node node)
        {
            (Guid, Guid) nodePosition = node.GetPosition();
            Connectors.RemoveAll(connector => connector.Origin == nodePosition || connector.Destination == nodePosition);
            return Nodes.Remove(node);
        }

        /// <summary>
        /// Deletes all nodes and connectors in a given row
        /// </summary>
        /// <param name="rowId"></param>
        public void DeleteAllContentsInRow(Guid rowId)
        {
            Nodes.RemoveAll(node => node.Y == rowId);
            Connectors.RemoveAll(connector => connector.Origin.Item2 == rowId || connector.Destination.Item2 == rowId);
        }

        /// <summary>
        /// Deletes all nodes and connectors in a given column
        /// </summary>
        /// <param name="colId"></param>
        public void DeleteAllContentsInColumn(Guid colId)
        {
            Console.WriteLine($"Removing all contents in column {colId}");
            Nodes.RemoveAll(node => node.X == colId);
            Connectors.RemoveAll(connector => connector.Origin.Item1 == colId || connector.Destination.Item1 == colId);

            Console.WriteLine($"{Nodes.Count} nodes remaining");
        }
    }
}
