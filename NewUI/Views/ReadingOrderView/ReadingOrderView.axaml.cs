using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using ROGraph.NewUI.Dialogs.ConfirmDialog;
using ROGraph.NewUI.Dialogs.EditNodeDialog;
using ROGraph.NewUI.Dispatchers;
using ROGraph.NewUI.Models;
using ROGraph.Shared.Enums;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Views.ReadingOrderView;

internal partial class ReadingOrderViewControl : UserControl
{
    private (Guid, Guid) newConnectorOrigin = (Guid.Empty, Guid.Empty);
    private InteractioMode interactioMode = InteractioMode.DEFAULT;

    public ReadingOrderViewControl(ReadingOrder readingOrder)
    {
        InitializeComponent();

        DataContext = new ReadingOrderViewModel(readingOrder);
    }

    private void HandleEmptySpaceNodeClick(object sender, PointerPressedEventArgs e)
    {
        e.Handled = true;

        if(e.Properties.IsRightButtonPressed)
        {
            var point =  e.GetCurrentPoint(sender as Control);
            OpenContextMenu(sender, point);
        }
    }

    private void HandleNodeLeftClick(object sender, RoutedEventArgs e)
    {
        if (this.interactioMode != InteractioMode.NEW_CONNECTOR)
        {
            return;
        }

        Control control = (Control)sender;
        if (control != null && control.DataContext is NodeModel node)
        {
            var destination = node.Node.GetPosition();
            var connector = new Connector(this.newConnectorOrigin, destination);
            ReadingOrderViewDispatcher.DispatchConnectorAddedMessage(connector);
            this.interactioMode = InteractioMode.DEFAULT;
        }
    }

    private void HandleNodeRightClick(object sender, PointerPressedEventArgs e)
    {
        // Has to be explictly marked as handled to prevent bubbling on Linux
        e.Handled = true;

        if(e.Properties.IsRightButtonPressed)
        {
            Control control = (Control)sender;
            if(control != null && control.DataContext is NodeModel node)
            {
                var point =  e.GetCurrentPoint(sender as Control);
                OpenContextMenu(node, point);
            }
            return;
        }
    }

    private void HandleConnectorClick(object sender, PointerPressedEventArgs e)
    {
        e.Handled = true;

        if(e.Properties.IsRightButtonPressed)
        {
            Control control = (Control)sender;
            if(control != null && control.DataContext is ConnectorModel connector)
            {
                var point =  e.GetCurrentPoint(sender as Control);
                OpenContextMenu(connector, point);
            }
            return;
        }
    }
    
    private void OpenContextMenu(object clicked, PointerPoint position)
    {
        ViewPanel.ContextMenu!.ItemsSource = null;
        IEnumerable<MenuItem> items = [];

        if(clicked is NodeModel)
        {
            var nodeModel = clicked as NodeModel ?? throw new ArgumentNullException(nameof(clicked));
            items = items.Concat(GetNodeContextMenuItems(nodeModel));
        }

        if(clicked is ConnectorModel connectorModel)
        {
            items = items.Concat(GetConnectorContextMenuItems(connectorModel));
        }
        
        if(clicked is Grid)
        {
            items = items.Concat(GetEmptySpaceContextMenuItems(position));
        }

        items = items.Concat(GetDefaultContextMenuItems(position));

        if(items.Any())
        {
            ViewPanel.ContextMenu!.ItemsSource = items;
        }
        else
        {
            Console.WriteLine("Menu needs to be closed as no items");
            // we close the menu here so it is properly cleared
            ViewPanel.ContextMenu!.Close();
        }
    }

    private IEnumerable<MenuItem> GetDefaultContextMenuItems(PointerPoint position)
    {
        var column = CoordinateUtils.GetColumnPosition(position.Position.X);
        var row = CoordinateUtils.GetColumnPosition(position.Position.Y);
        return
        [
            new MenuItem{ Header = "Add New Column", Command = this.AddColumnCommand, CommandParameter = column},
            new MenuItem{ Header = "Add New Row", Command = this.AddRowCommand, CommandParameter = row},
            new MenuItem{ Header = "Delete Column", Command = this.DeleteColumnCommand, CommandParameter = column},
            new MenuItem{ Header = "Delete Row", Command = this.DeleteRowCommand, CommandParameter = row}
        ];
    }

    private IEnumerable<MenuItem> GetEmptySpaceContextMenuItems(PointerPoint position)
    {
        var nearestNodePosition = CoordinateUtils.GetNearestValidNodePosition((position.Position.X, position.Position.Y));
        return
        [
            new MenuItem{ Header = "Add Node", Command = this.CreateNewNodeCommand, CommandParameter = nearestNodePosition}
        ];
    }

    private IEnumerable<MenuItem> GetNodeContextMenuItems(NodeModel node)
    {
        return
        [
            new MenuItem{ Header = "Edit Node", Command = this.OpenEditDialogCommand, CommandParameter = node.Node},
            new MenuItem{ Header = "Delete Node", Command = this.OpenDeleteDialogCommand, CommandParameter = node.Node},
            new MenuItem{ Header = "New Connector", Command = this.SetNewConnectorOriginCommand, CommandParameter = node.Node.GetPosition()}
        ];
    }

    private IEnumerable<MenuItem> GetConnectorContextMenuItems(ConnectorModel connectorModel)
    {
        return
        [
            new MenuItem{ Header = "Delete Connector", Command = this.DeleteConnectorCommand, CommandParameter = connectorModel.Id}
        ];
    }

    [RelayCommand]
    private async Task CreateNewNode((int, int) position)
    {
        var node = new Node(
            Guid.NewGuid(), 
            "New Node", 
            Guid.Empty, 
            DateTime.Now, 
            DateTime.Now, 
            Guid.Empty,
            Guid.Empty,
            NodeType.Triangle,
            false,
            null,
            string.Empty);
        NodeModel model = new(node, position.Item1, position.Item2);
        ReadingOrderViewDispatcher.DispatchNodeAddedMessage(model);

        bool result = await EditNode(node, true);

        if(!result)
        {
            ReadingOrderViewDispatcher.DispatchNodeDeletedMessage(node.Id);
        }
    }

    [RelayCommand]
    private async Task OpenDeleteDialog(Node node)
    {
        var dialog = new ConfirmDialogView($"Are you sure you want to delete {node.Name}?");
        var root = this.VisualRoot as Window;
        bool shouldDelete = await dialog.ShowDialog<bool>(root!);

        if(shouldDelete)
        {
            ReadingOrderViewDispatcher.DispatchNodeDeletedMessage(node.Id);
        }
    }

    [RelayCommand]
    private async Task OpenEditDialog(Node node)
    {
        await EditNode(node);
    }

    [RelayCommand]
    private void SetNewConnectorOrigin((Guid, Guid) position)
    {
        this.newConnectorOrigin = position;
        this.interactioMode = InteractioMode.NEW_CONNECTOR;
    }

    [RelayCommand]
    private void DeleteConnector(Guid id)
    {
        ReadingOrderViewDispatcher.DispatchConnectorDeletedMessage(id);
    }

    private async Task<bool> EditNode(Node node, bool confirmCancel = false)
    {
        var dialog = new EditNodeDialogView(node, confirmCancel);
        var root = this.VisualRoot as Window;
        bool result = await dialog.ShowDialog<bool>(root!);

        return result;
    }

    [RelayCommand]
    private void AddColumn(int position)
    {
        ReadingOrderViewDispatcher.DispatchColumnAddedEvent(position);
        this.InvalidateVisual();
    }

    [RelayCommand]
    private async Task DeleteColumn(int position)
    {
        var dialog = new ConfirmDialogView($"Are you sure you want to delete this column?");
        var root = this.VisualRoot as Window;
        bool shouldDelete = await dialog.ShowDialog<bool>(root!);

        if(!shouldDelete)
        {
            return;
        }

        Console.WriteLine($"Deleting column {position}");
        ReadingOrderViewDispatcher.DispatchColumnDeletedEvent(position);
        this.InvalidateVisual();
    }

    [RelayCommand]
    private void AddRow(int position)
    {
        ReadingOrderViewDispatcher.DispatchRowAddedEvent(position);
        this.InvalidateVisual();
    }

    [RelayCommand]
    private async Task DeleteRow(int position)
    {
        var dialog = new ConfirmDialogView($"Are you sure you want to delete this row?");
        var root = this.VisualRoot as Window;
        bool shouldDelete = await dialog.ShowDialog<bool>(root!);

        if(!shouldDelete)
        {
            return;
        }

        ReadingOrderViewDispatcher.DispatchRowDeletedEvent(position);
        this.InvalidateVisual();
    }
}