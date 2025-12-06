using System.Collections.ObjectModel;
using ROGraph.NewUI.ViewModels;
using ROGraph.Shared.Models;
using ROGraph.NewUI.Models;
using System;

namespace ROGraph.NewUI.Views.ReadingOrderView;

internal partial class ReadingOrderViewModel : ViewModelBase
{
    const int IMAGE_SIZE = 240;
    const int IMAGE_GAP_SIZE = 80;

    public ReadingOrder ReadingOrder { get; set; }

    public ObservableCollection<NodeModel> Nodes { get; set; } = [];

    public ObservableCollection<ConnectorModel> Connectors { get; set; } = [];

    public ReadingOrderViewModel(ReadingOrder readingOrder)
    {
        ArgumentNullException.ThrowIfNull(readingOrder.CoordinateTranslator, nameof(readingOrder.CoordinateTranslator));

        this.ReadingOrder = readingOrder;

        var nodes = readingOrder.Contents.GetNodes();
        var connectors = readingOrder.Contents.GetConnectors();

        foreach(Node n in nodes)
        {
            (int, int) position = GetNodePosition(readingOrder.CoordinateTranslator.Translate(n));
            Nodes.Add(new NodeModel(n, position.Item1, position.Item2));

            Console.WriteLine($"Added Node at {position}");
        }

        foreach(Connector c in connectors)
        {
            (int, int) origin = this.ReadingOrder.CoordinateTranslator.Translate(c.origin);
            (int, int) destination = this.ReadingOrder.CoordinateTranslator.Translate(c.destination);

            var positions = GetConnectorPositions(origin, destination);

            Connectors.Add(new ConnectorModel(c, positions.Item1, positions.Item2));

            Console.WriteLine($"Added Connector at {positions}");
        }
    }

    private static (int, int) GetNodePosition((int, int) position)
    {
        int x = position.Item1;
        int y = position.Item2;

        x = (x * IMAGE_SIZE) + (x * IMAGE_GAP_SIZE) + (IMAGE_GAP_SIZE / 2);
        y = (y * IMAGE_SIZE) + (y * (IMAGE_GAP_SIZE / 2)) + (IMAGE_GAP_SIZE / 4);

        return (x, y);
    }

    private static ((int, int), (int, int)) GetConnectorPositions((int, int) origin, (int, int) destination)
    {
        int x1 = origin.Item1;
        int y1 = origin.Item2;
        int x2 = destination.Item1;
        int y2 = destination.Item2;

        x1 = (x1 * IMAGE_SIZE) + (x1 * IMAGE_GAP_SIZE) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 2);
        y1 = (y1 * IMAGE_SIZE) + (y1 * (IMAGE_GAP_SIZE / 2)) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 4);
        x2 = (x2 * IMAGE_SIZE) + (x2 * IMAGE_GAP_SIZE) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 2);
        y2 = (y2 * IMAGE_SIZE) + (y2 * (IMAGE_GAP_SIZE / 2)) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 4);

        return ((x1, y1), (x2, y2));
    }
}