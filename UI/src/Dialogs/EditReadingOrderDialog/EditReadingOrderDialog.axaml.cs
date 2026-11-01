using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Dialogs.EditReadingOrderDialog;

public partial class EditReadingOrderDialog : Window
{
    public EditReadingOrderDialog(ReadingOrderOverview readingOrderOverview, IMessenger messenger)
    {
        InitializeComponent();
        
        this.DataContext = new EditReadingOrderDialogViewModel(readingOrderOverview, messenger);
        
        messenger.Register<EditReadingOrderDialogClosedMessage>(this, (r, m) =>
        {
            this.Close(m.ReadingOrderOverview);
        });
    }
}