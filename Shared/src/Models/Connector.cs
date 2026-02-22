namespace ROGraph.Shared.Models;

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

public class ConnectorComparer : IEqualityComparer<Connector>
{
    public bool Equals(Connector? x, Connector? y)
    {
        if (ReferenceEquals(x, y)) return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
        
        return x.Origin == y.Origin && x.Destination == y.Destination;
    }

    public int GetHashCode(Connector connector)
    {
        if (ReferenceEquals(connector, null)) return 0;

        var originHashCode = connector.Origin.GetHashCode();
        var destinationHashCode = connector.Destination.GetHashCode();
        return originHashCode ^ destinationHashCode;
    }
}