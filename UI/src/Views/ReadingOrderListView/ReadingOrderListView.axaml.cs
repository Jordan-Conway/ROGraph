using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ROGraph.UI.Views.ReadingOrderView;
using ReactiveUI;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Backend.Contracts;
using ROGraph.Backend.DataProviders.SQLiteProviders;
using ROGraph.Shared.Models;
using ROGraph.UI.Dialogs.EditReadingOrderDialog;
using ROGraph.UI.Dispatchers;
using ROGraph.UI.Messages;

namespace ROGraph.UI.Views.ReadingOrderListView;

internal partial class ReadingOrderListViewControl : UserControl
{
    public ReactiveCommand<Guid, Unit> NavigateCommand { get; }

    public ReadingOrderListViewControl()
    {
        InitializeComponent();

        NavigateCommand = ReactiveCommand.CreateFromTask<Guid>(NavigateToReadingOrder);

        RegisterMessages();
    }

    [RelayCommand]
    public void CreateReadingOrder()
    {
        var overview = new ReadingOrderOverview("New", Guid.NewGuid());
        ReadingOrderListViewDispatcher.DispatchReadingOrderAddedMessage(overview);
        this.InvalidateVisual();
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
