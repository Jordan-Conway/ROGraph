using System.Collections.ObjectModel;
using ROGraph.NewUI.ViewModels;
using ROGraph.Shared.Models;
using ROGraph.NewUI.Models;
using System;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.NewUI.Messages;
using DynamicData;
using System.Linq;
using System.Reactive.Linq;

namespace ROGraph.NewUI.Views.ReadingOrderView;

internal partial class ReadingOrderViewModel : ViewModelBase
{
    public ReadingOrder ReadingOrder { get; set; }

    public ObservableCollection<NodeModel> Nodes { get; set; } = [];

    public ObservableCollection<ConnectorModel> Connectors { get; set; } = [];

    public ReadingOrderViewModel(ReadingOrder readingOrder)
    {
        ArgumentNullException.ThrowIfNull(readingOrder.CoordinateTranslator, nameof(readingOrder.CoordinateTranslator));

        this.ReadingOrder = readingOrder;

        RegisterMessages();
        PlaceContents();
    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<NodeAddedMessage>(this, (r,m) =>
        {
            this.AddNode(m.Value);
        });
        WeakReferenceMessenger.Default.Register<NodeDeletedMessage>(this, (r, m) =>
        {
            this.DeleteNode(m.Value);
        });
        WeakReferenceMessenger.Default.Register<NodeEditedMessage>(this, (r,m) =>
        {
            this.EditNode(m.Value);
        });
        WeakReferenceMessenger.Default.Register<ConnectorAddedMessage>(this, (r, m) =>
        {
            this.AddConnector(m.Value);
        });
        WeakReferenceMessenger.Default.Register<ConnectorDeletedMessage>(this, (r, m) =>
        {
            this.DeleteConnector(m.Value);
        });
        WeakReferenceMessenger.Default.Register<ColumnAddedMessage>(this, (r, m) =>
        {
           this.AddColumn(m.Value); 
        });
        WeakReferenceMessenger.Default.Register<ColumnDeletedMessage>(this, (r, m) =>
        {
           this.DeleteColumn(m.Value);
        });
        WeakReferenceMessenger.Default.Register<RowAddedMessage>(this, (r,m) =>
        {
           this.AddRow(m.Value); 
        });
        WeakReferenceMessenger.Default.Register<RowDeletedMessage>(this, (r,m) =>
        {
           this.DeleteRow(m.Value); 
        });
    }

    private void PlaceContents()
    {
        if(this.ReadingOrder.CoordinateTranslator == null)
        {
            throw new InvalidOperationException("Cannot place contents without coordinate translator");
        }

        foreach(Node n in this.ReadingOrder.Contents.GetNodes())
        {
            Console.WriteLine(n.Name);
            (int, int) position =  CoordinateUtils.GetNodePosition(this.ReadingOrder.CoordinateTranslator.Translate(n));
            Nodes.Add(new NodeModel(n, position.Item1, position.Item2));
        }

        foreach(Connector c in this.ReadingOrder.Contents.GetConnectors())
        {
            (int, int) origin = this.ReadingOrder.CoordinateTranslator.Translate(c.origin);
            (int, int) destination = this.ReadingOrder.CoordinateTranslator.Translate(c.destination);

            var positions = CoordinateUtils.GetConnectorPositions(origin, destination);

            Connectors.Add(new ConnectorModel(c, positions.Item1, positions.Item2));
        }
    }

    private void RemoveAllContents()
    {
        this.Nodes.Clear();
        this.Connectors.Clear();
    }

    private void AddNode(NodeModel nodeModel)
    {
        if(this.ReadingOrder.CoordinateTranslator == null)
        {
            throw new InvalidOperationException("Cannot add node without coordinate translator");
        }

        nodeModel.Node.Origin = this.ReadingOrder.Id;
        var gridPosition = CoordinateUtils.GetNodeCoordinates((nodeModel.X, nodeModel.Y));
        nodeModel.Node.X = this.ReadingOrder.CoordinateTranslator.GetXFromInt(gridPosition.Item1);
        nodeModel.Node.Y = this.ReadingOrder.CoordinateTranslator.GetYFromInt(gridPosition.Item2);

        Console.WriteLine($"Added node with x: {nodeModel.Node.X}, y: {nodeModel.Node.Y}");

        this.Nodes.Add(nodeModel);
        this.ReadingOrder.Contents.AddNode(nodeModel.Node);
    }

    private void DeleteNode(Guid id)
    {
        var toRemove = this.Nodes.Where(node => node.Node.Id == id);
        this.Nodes.RemoveMany(toRemove);
        foreach(NodeModel nodeModel in toRemove)
        {
            this.ReadingOrder.Contents.DeleteNode(nodeModel.Node);
        }
    }

    private void EditNode(Node node)
    {
        NodeModel? existing = this.Nodes.FirstOrDefault(n => n.Node.Id == node.Id);
        if(existing == null)
        {
            Console.WriteLine("Tried to edit nonexistant node");
            return;
        }

        NodeModel newModel = new(node, existing.X, existing.Y);
        this.DeleteNode(node.Id);
        this.Nodes.Add(newModel);
        this.ReadingOrder.Contents.ReplaceNode(node);
    }

    private void AddConnector(Connector connector)
    {
        if(this.ReadingOrder.CoordinateTranslator == null)
        {
            throw new InvalidOperationException("Cannot add connectorwithout coordinate translator");
        }

        var origin = this.ReadingOrder.CoordinateTranslator.Translate(connector.origin);
        var destination = this.ReadingOrder.CoordinateTranslator.Translate(connector.destination);

        var positions = CoordinateUtils.GetConnectorPositions(origin, destination);

        this.Connectors.Add(new ConnectorModel(connector, positions.Item1, positions.Item2));
        this.ReadingOrder.Contents.AddConnector(connector);
    }

    private void DeleteConnector(Guid id)
    {
        var toRemove = this.Connectors.Where(c => c.Id == id);
        this.Connectors.RemoveMany(toRemove);
        foreach(ConnectorModel connectorModel in toRemove)
        {
            this.ReadingOrder.Contents.DeleteConnector(connectorModel.Connector.origin, connectorModel.Connector.destination);
        }
    }

    private void AddColumn(int position)
    {
        this.ReadingOrder.AddColumn(position);
        this.RefreshContents();
    }

    private void DeleteColumn(int position)
    {
        this.ReadingOrder.DeleteColumn(position);
        this.RefreshContents();
    }

    private void AddRow(int position)
    {
        this.ReadingOrder.AddRow(position);
        this.RefreshContents();
    }

    private void DeleteRow(int position)
    {
        this.ReadingOrder.DeleteRow(position);
        this.RefreshContents();
    }

    /// <summary>
    /// Refreshes this classess contents
    /// </summary>
    private void RefreshContents()
    {
        this.RemoveAllContents();
        this.PlaceContents();
    }
}