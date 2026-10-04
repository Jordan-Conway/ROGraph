using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ROGraph.UI.Views.ReadingOrderView;
using ReactiveUI;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Backend.Contracts;
using ROGraph.Shared.Models;
using ROGraph.UI.Dialogs.EditReadingOrderDialog;
using ROGraph.UI.Messages;

namespace ROGraph.UI.Views.ReadingOrderListView;

public partial class ReadingOrderListView : UserControl
{
    public ReactiveCommand<Guid, Unit> NavigateCommand { get; }

    public ReadingOrderListView(ReadingOrderListViewModel viewModel)
    {
        InitializeComponent();
        
        DataContext = viewModel;

        NavigateCommand = ReactiveCommand.CreateFromTask<Guid>(NavigateToReadingOrder);

        RegisterMessages();
    }

    private static async Task NavigateToReadingOrder(Guid id)
    {

    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<EditReadingOrderMessage>(this,  (r, m) =>
        {
            var dialog = new EditReadingOrderDialog(m.Overview);
            var root = this.VisualRoot as Window;
            m.Reply(dialog.ShowDialog<ReadingOrderOverview?>(root!));
        });
    }
}
