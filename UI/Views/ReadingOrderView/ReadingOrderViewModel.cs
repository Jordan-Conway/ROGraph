using System.Collections.ObjectModel;
using ROGraph.UI.ViewModels;
using ROGraph.Shared.Models;
using ROGraph.UI.Models;
using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Messages;
using DynamicData;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using ROGraph.UI.Dispatchers;

namespace ROGraph.UI.Views.ReadingOrderView;

internal partial class ReadingOrderViewModel : ViewModelBase
{
    private ReadingOrder ReadingOrder { get; set; }

    public ObservableCollection<NodeModel> Nodes { get; set; } = [];

    public ObservableCollection<ConnectorModel> Connectors { get; set; } = [];
    
    public int CanvasWidth => (ReadingOrder.CoordinateTranslator!.GetNumberOfColumns() + 2) * Sizes.ColumnWidth;
    public int CanvasHeight => (ReadingOrder.CoordinateTranslator!.GetNumberOfRows() +1) * Sizes.ColumnWidth;

    public ReadingOrderViewModel(ReadingOrder readingOrder)
    {
        ArgumentNullException.ThrowIfNull(readingOrder.CoordinateTranslator, nameof(readingOrder.CoordinateTranslator));

        ReadingOrder = readingOrder;

        RegisterMessages();
        PlaceContents();
    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<NodeAddedMessage>(this, (r,m) =>
        {
            AddNode(m.Value);
        });
        WeakReferenceMessenger.Default.Register<NodeDeletedMessage>(this, (r, m) =>
        {
            DeleteNode(m.Value);
        });
        WeakReferenceMessenger.Default.Register<NodeEditedMessage>(this, (r,m) =>
        {
            EditNode(m.Value);
        });
        WeakReferenceMessenger.Default.Register<ConnectorAddedMessage>(this, (r, m) =>
        {
            AddConnector(m.Value);
        });
        WeakReferenceMessenger.Default.Register<ConnectorDeletedMessage>(this, (r, m) =>
        {
            DeleteConnector(m.Value);
        });
        WeakReferenceMessenger.Default.Register<ColumnAddedMessage>(this, (r, m) =>
        {
           AddColumn(m.Value); 
        });
        WeakReferenceMessenger.Default.Register<ColumnDeletedMessage>(this, (r, m) =>
        {
           DeleteColumn(m.Value);
        });
        WeakReferenceMessenger.Default.Register<RowAddedMessage>(this, (r,m) =>
        {
           AddRow(m.Value); 
        });
        WeakReferenceMessenger.Default.Register<RowDeletedMessage>(this, (r,m) =>
        {
           DeleteRow(m.Value); 
        });
    }

    private void PlaceContents()
    {
        if(ReadingOrder.CoordinateTranslator == null)
        {
            throw new InvalidOperationException("Cannot place contents without coordinate translator");
        }

        foreach(Node n in ReadingOrder.Contents.GetNodes())
        {
            Console.WriteLine(n.Name);
            (int, int) position =  CoordinateUtils.GetNodePosition(ReadingOrder.CoordinateTranslator.Translate(n));
            Nodes.Add(new NodeModel(n, position.Item1, position.Item2));
        }

        foreach(Connector c in ReadingOrder.Contents.GetConnectors())
        {
            (int, int) origin = ReadingOrder.CoordinateTranslator.Translate(c.Origin);
            (int, int) destination = ReadingOrder.CoordinateTranslator.Translate(c.Destination);

            var positions = CoordinateUtils.GetConnectorPositions(origin, destination);

            Connectors.Add(new ConnectorModel(c, positions.Item1, positions.Item2));
        }
    }

    private void RemoveAllContents()
    {
        Nodes.Clear();
        Connectors.Clear();
    }

    private void AddNode(NodeModel nodeModel)
    {
        if(ReadingOrder.CoordinateTranslator == null)
        {
            throw new InvalidOperationException("Cannot add node without coordinate translator");
        }

        nodeModel.Node.Origin = ReadingOrder.Id;
        var gridPosition = CoordinateUtils.GetNodeCoordinates((nodeModel.X, nodeModel.Y));
        nodeModel.Node.X = ReadingOrder.CoordinateTranslator.GetXFromInt(gridPosition.Item1);
        nodeModel.Node.Y = ReadingOrder.CoordinateTranslator.GetYFromInt(gridPosition.Item2);

        Console.WriteLine($"Added node with x: {nodeModel.Node.X}, y: {nodeModel.Node.Y}");

        Nodes.Add(nodeModel);
        ReadingOrder.Contents.AddNode(nodeModel.Node);
        UpdateCanvasSize();
    }

    private void DeleteNode(Guid id)
    {
        var toRemove = Nodes.Where(node => node.Node.Id == id).ToList();
        Nodes.RemoveMany(toRemove);
        foreach(var nodeModel in toRemove)
        {
            ReadingOrder.Contents.DeleteNode(nodeModel.Node);
        }
    }

    private void EditNode(Node node)
    {
        NodeModel? existing = Nodes.FirstOrDefault(n => n.Node.Id == node.Id);
        if(existing == null)
        {
            Console.WriteLine("Tried to edit nonexistent node");
            return;
        }

        NodeModel newModel = new(node, existing.X, existing.Y);
        DeleteNode(node.Id);
        Nodes.Add(newModel);
        ReadingOrder.Contents.AddNode(newModel.Node);
    }

    private void AddConnector(Connector connector)
    {
        if(ReadingOrder.CoordinateTranslator == null)
        {
            throw new InvalidOperationException("Cannot add connector without coordinate translator");
        }

        var origin = ReadingOrder.CoordinateTranslator.Translate(connector.Origin);
        var destination = ReadingOrder.CoordinateTranslator.Translate(connector.Destination);

        var positions = CoordinateUtils.GetConnectorPositions(origin, destination);

        Connectors.Add(new ConnectorModel(connector, positions.Item1, positions.Item2));
        ReadingOrder.Contents.AddConnector(connector);
    }

    private void DeleteConnector(Guid id)
    {
        var toRemove = Connectors.Where(c => c.Id == id).ToList();
        Connectors.RemoveMany(toRemove);
        foreach(var connectorModel in toRemove)
        {
            ReadingOrder.Contents.DeleteConnector(connectorModel.Connector.Origin, connectorModel.Connector.Destination);
        }
    }

    private void AddColumn(int position)
    {
        ReadingOrder.AddColumn(position);
        UpdateCanvasSize();
        RefreshContents();
    }

    private void DeleteColumn(int position)
    {
        ReadingOrder.DeleteColumn(position);
        UpdateCanvasSize();
        RefreshContents();
    }

    private void AddRow(int position)
    {
        ReadingOrder.AddRow(position);
        UpdateCanvasSize();
        RefreshContents();
    }

    private void DeleteRow(int position)
    {
        ReadingOrder.DeleteRow(position);
        UpdateCanvasSize();
        RefreshContents();
    }
    
    [RelayCommand]
    private async Task Save()
    {
        var result = await ReadingOrderViewDispatcher.DispatchSaveReadingOrderEvent(ReadingOrder);
        Debug.WriteLine(!result ? "Failed to save reading order" : "Successfully saved reading order");
    }

    /// <summary>
    /// Refreshes this classes contents
    /// </summary>
    private void RefreshContents()
    {
        RemoveAllContents();
        PlaceContents();
    }

    private void UpdateCanvasSize()
    {
        IReactiveObject reactiveObject =  this;
        reactiveObject.RaisePropertyChanged(nameof(CanvasWidth));
        reactiveObject.RaisePropertyChanged(nameof(CanvasHeight));
    }
}