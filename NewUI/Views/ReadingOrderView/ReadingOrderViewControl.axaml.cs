using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using ROGraph.NewUI.Dialogs.EditNodeDialog;
using ROGraph.NewUI.Dispatchers;
using ROGraph.NewUI.Models;
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
        Console.WriteLine("Clicked empty space");
        if(e.Properties.IsRightButtonPressed)
        {
            OpenContextMenu(sender);
        }
    }

    private void HandleNodeClick(object sender, PointerPressedEventArgs e)
    {
        if(e.Properties.IsRightButtonPressed)
        {
            Control control = (Control)sender;
            if(control != null && control.DataContext is NodeModel node)
            {
                OpenContextMenu(node);
            }
        }

        e.Handled = true;
    }

    // TODO: Remove e if it is unused
    private void OpenContextMenu(object clicked)
    {
        ViewPanel.ContextMenu!.ItemsSource = null;
        IEnumerable<MenuItem> items = [];
        
        Console.WriteLine(clicked);

        if(clicked is NodeModel)
        {
            var nodeModel = clicked as NodeModel ?? throw new ArgumentNullException(nameof(clicked));
            items = items.Concat(GetNodeContextMenuItems(nodeModel));
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

    private IEnumerable<MenuItem> GetEmptySpaceContextMenuItems()
    {
        return
        [
            new MenuItem{ Header = "Default"}
        ];
    }

    private IEnumerable<MenuItem> GetNodeContextMenuItems(NodeModel node)
    {
        Console.WriteLine("Clicked on a node!");
        return
        [
            new MenuItem{ Header = "Edit Node", Command = this.OpenEditDialogCommand, CommandParameter = node},
            new MenuItem{ Header = "Delete Node", Command = ReadingOrderViewDispatcher.DispatchNodeDeletedCommand, CommandParameter = node.Node.Id}
        ];
    }

    [RelayCommand]
    private void OpenEditDialog(NodeModel nodeModel)
    {
        Console.WriteLine(nodeModel.Node.Name);
        var dialog = new EditNodeDialogView(nodeModel);
        var root = this.VisualRoot as Window;
        Console.WriteLine(root);
        dialog.ShowDialog(root!);
    }
}