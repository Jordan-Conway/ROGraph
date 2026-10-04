using CommunityToolkit.Mvvm.ComponentModel;
using ROGraph.UI.Views.ReadingOrderListView;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Messages;
using System;
using System.Data.SQLite;
using ROGraph.Backend.Contracts;
using ROGraph.UI.Services;

namespace ROGraph.UI;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private UserControl _currentPage;
    private readonly IMessagingService _messagingService;

    public MainWindowViewModel(ReadingOrderListView initialPage, IMessagingService messagingService)
    {
        _currentPage  = initialPage;
        _messagingService = messagingService;
        
        this.RegisterMessages();
    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) =>
        {
            this.ChangeView(m.Value);
        });
    }

    private void ChangeView(UserControl userControl)
    {
        Console.WriteLine("Moving to new view");
        CurrentPage = userControl;
    }
}
