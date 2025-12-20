using ROGraph.Shared.Models;
using Avalonia;
using System;
using Avalonia.Media;

namespace ROGraph.NewUI.Models;

public class ConnectorModel
{
    public Connector Connector { get; set; }
    public Guid Id { get; set; }

    public Point Origin { get; set; }

    public Point Destination { get; set; }

    public Geometry Geometry => new LineGeometry(Origin, Destination);

    public ConnectorModel(Connector connector, (int, int) origin, (int, int) destination)
    {
        this.Connector = connector;
        this.Id = Guid.NewGuid();
        this.Origin = new Point(origin.Item1, origin.Item2);
        this.Destination = new Point(destination.Item1, destination.Item2);
    }
}