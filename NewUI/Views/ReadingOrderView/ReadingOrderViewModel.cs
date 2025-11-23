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

    public ReadingOrderViewModel(ReadingOrder readingOrder)
    {
        this.ReadingOrder = readingOrder;

        foreach(Node n in readingOrder.Contents.GetNodes())
        {
            (int, int) position = GetPosition(readingOrder.CoordinateTranslator.Translate(n));
            Nodes.Add(new NodeModel(n, position.Item1, position.Item2));
        }
    }

    private (int, int) GetPosition((int, int) position)
    {
        int x = position.Item1;
        int y = position.Item2;

        x = (x * IMAGE_SIZE) + (x * IMAGE_GAP_SIZE) + (IMAGE_GAP_SIZE / 2);
        y = (y * IMAGE_SIZE) + (y * (IMAGE_GAP_SIZE / 2)) + (IMAGE_GAP_SIZE / 4);

        return (x, y);
    }
}