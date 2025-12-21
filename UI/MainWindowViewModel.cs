using CommunityToolkit.Mvvm.ComponentModel;
using ROGraph.UI.Views.ReadingOrderListView;
using ROGraph.UI.ViewModels;
using ROGraph.Backend.Data.DataProviders.Interfaces;
using ROGraph.Backend.Data.DataProviders.MockProviders;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Messages;
using System;

namespace ROGraph.UI;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private UserControl _currentPage;

    public MainWindowViewModel()
    {
        _currentPage  = new ReadingOrderListViewControl();

        this.RegisterMessages();
    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) =>
        {
            this.ChangeView(m.Value);
        });
    }

    public void ChangeView(UserControl userControl)
    {
        Console.WriteLine("Moving to new view");
        CurrentPage = userControl;
    }
}
