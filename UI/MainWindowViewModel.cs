using CommunityToolkit.Mvvm.ComponentModel;
using ROGraph.UI.Views.ReadingOrderListView;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Messages;
using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using ROGraph.Backend.DataProviders.Interfaces;

namespace ROGraph.UI;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private UserControl _currentPage;
    private readonly IReadingOrderProvider _readingOrderProvider;

    public MainWindowViewModel(IReadingOrderProvider readingOrderProvider)
    {
        _currentPage  = new ReadingOrderListViewControl();
        _readingOrderProvider = readingOrderProvider;
        
        this.RegisterMessages();
    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) =>
        {
            this.ChangeView(m.Value);
        });
        WeakReferenceMessenger.Default.Register<SaveReadingOrderMessage>(this, (r, m) =>
        {
            try
            {
                m.Reply(_readingOrderProvider.UpdateReadingOrder(m.ReadingOrder));
            }
            catch (SQLiteException)
            {
                m.Reply(false);
            }
        });
    }

    private void ChangeView(UserControl userControl)
    {
        Console.WriteLine("Moving to new view");
        CurrentPage = userControl;
    }
}
