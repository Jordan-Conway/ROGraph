using CommunityToolkit.Mvvm.ComponentModel;
using ROGraph.UI.Views.ReadingOrderListView;
using ROGraph.UI.ViewModels;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Messages;
using System;
using ROGraph.Backend.DataProviders.Interfaces;

namespace ROGraph.UI;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private UserControl _currentPage;
    private IReadingOrderProvider _readingOrderProvider;

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
            _readingOrderProvider.UpdateReadingOrder(m.Value);
        });
    }

    public void ChangeView(UserControl userControl)
    {
        Console.WriteLine("Moving to new view");
        CurrentPage = userControl;
    }
}
