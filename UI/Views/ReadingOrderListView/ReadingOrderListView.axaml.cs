using System;
using System.Reactive;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ROGraph.UI.Views.ReadingOrderView;
using ReactiveUI;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.Backend.DataProviders.SQLiteProviders;
using ROGraph.Shared.Models;
using ROGraph.UI.Dispatchers;
using ROGraph.UI.Messages;

namespace ROGraph.UI.Views.ReadingOrderListView;

internal partial class ReadingOrderListViewControl : UserControl
{
    private static readonly IReadingOrderProvider RoProvider = new ReadingOrderListProvider();

    public ReactiveCommand<Guid, Unit> NavigateCommand { get; }

    public ReadingOrderListViewControl()
    {
        InitializeComponent();

        DataContext = new ReadingOrderListViewModel(RoProvider);

        NavigateCommand = ReactiveCommand.Create<Guid>(NavigateToReadingOrder);
    }

    [RelayCommand]
    public void CreateReadingOrder()
    {
        var overview = new ReadingOrderOverview("New", Guid.NewGuid());
        ReadingOrderListViewDispatcher.DispatchReadingOrderAddedMessage(overview);
        this.InvalidateVisual();
    }

    private static void NavigateToReadingOrder(Guid id)
    {
        var readingOrder = RoProvider.GetReadingOrder(id);
        ArgumentNullException.ThrowIfNull(readingOrder);
        
        WeakReferenceMessenger.Default.Send(new NavigationMessage(new ReadingOrderViewControl(readingOrder)));
    }
}