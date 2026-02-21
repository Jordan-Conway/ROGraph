namespace ROGraph.Shared.Models
{
    public class Connector
    {
        public (Guid, Guid) Origin { get; set; }
        public (Guid, Guid) Destination { get; set; }

        public Connector((Guid, Guid) origin, (Guid, Guid) destination)
        {
            this.Origin = origin;
            this.Destination = destination;
        }
    }
}
