using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using ROGraph.NewUI.Dialogs.DeleteConfirmDialog;
using ROGraph.NewUI.Dialogs.EditNodeDialog;
using ROGraph.NewUI.Dispatchers;
using ROGraph.NewUI.Models;
using ROGraph.Shared.Enums;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Views.ReadingOrderView;

internal partial class ReadingOrderViewControl : UserControl
{
    public ReadingOrderViewControl(ReadingOrder readingOrder)
    {
        InitializeComponent();

        DataContext = new ReadingOrderViewModel(readingOrder);
    }

    private void HandleEmptySpaceNodeClick(object sender, PointerPressedEventArgs e)
    {
        if(e.Properties.IsRightButtonPressed)
        {
            var point =  e.GetCurrentPoint(sender as Control);
            OpenContextMenu(sender, point);
        }
    }

    private void HandleNodeClick(object sender, PointerPressedEventArgs e)
    {
        if(e.Properties.IsRightButtonPressed)
        {
            Control control = (Control)sender;
            if(control != null && control.DataContext is NodeModel node)
            {
                var point =  e.GetCurrentPoint(sender as Control);
                OpenContextMenu(node, point);
            }
        }

        e.Handled = true;
    }

    private void OpenContextMenu(object clicked, PointerPoint position)
    {
        ViewPanel.ContextMenu!.ItemsSource = null;
        IEnumerable<MenuItem> items = [];
        
        Console.WriteLine(clicked);

        if(clicked is NodeModel)
        {
            var nodeModel = clicked as NodeModel ?? throw new ArgumentNullException(nameof(clicked));
            items = items.Concat(GetNodeContextMenuItems(nodeModel));
        }
        
        if(clicked is Grid)
        {
            items = items.Concat(GetEmptySpaceContextMenuItems(position));
        }

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
            new MenuItem{ Header = "Delete Node", Command = this.OpenDeleteDialogCommand, CommandParameter = node.Node}
        ];
    }

    [RelayCommand]
    private void CreateNewNode((int, int) position)
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

        OpenEditDialog(node);
    }

    [RelayCommand]
    private async Task OpenDeleteDialog(Node node)
    {
        var dialog = new DeleteConfirmDialogView($"{node.Name}");
        var root = this.VisualRoot as Window;
        bool shouldDelete = await dialog.ShowDialog<bool>(root!);

        if(shouldDelete)
        {
            ReadingOrderViewDispatcher.DispatchNodeDeletedMessage(node.Id);
        }
    }

    [RelayCommand]
    private void OpenEditDialog(Node node)
    {
        Console.WriteLine(node.Name);
        var dialog = new EditNodeDialogView(node);
        var root = this.VisualRoot as Window;
        Console.WriteLine(root);
        dialog.ShowDialog(root!);
    }
}