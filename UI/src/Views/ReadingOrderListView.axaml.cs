using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Shared.Models;
using ROGraph.UI.Dialogs.EditReadingOrderDialog;
using ROGraph.UI.Messages;
using ROGraph.UI.Pages;
using ROGraph.UI.ViewModels;

namespace ROGraph.UI.Views;

public partial class ReadingOrderListView : Page
{
    private readonly IMessenger _messenger;
    
    public ReadingOrderListView(ReadingOrderListViewModel viewModel, IMessenger messenger)
    {
        InitializeComponent();

        DataContext = viewModel;

        _messenger  = messenger;
        RegisterMessages();
    }

    private void RegisterMessages()
    {
        _messenger.Register<EditReadingOrderMessage>(this, (r, m) =>
        {
            var dialog = new EditReadingOrderDialog(m.Overview, _messenger);
            var root = this.VisualRoot as Window;
            m.Reply(dialog.ShowDialog<ReadingOrderOverview?>(root!));
        });
    }
}
