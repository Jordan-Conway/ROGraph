using System;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ROGraph.UI.Views.ReadingOrderView;
using ROGraph.Shared.Models;
using ReactiveUI;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.Backend.DataProviders.MockProviders;
using ROGraph.UI.Messages;

namespace ROGraph.UI.Views.ReadingOrderListView;

internal partial class ReadingOrderListViewControl : UserControl
{
    private static readonly IReadingOrderListProvider ListProvider = new MockReadingOrderListProvider();
    private static readonly IReadingOrderProvider RoProvider = new MockReadingOrderProvider();

    public ReactiveCommand<ReadingOrderOverview, Unit> NavigateCommand { get; }

    public ReadingOrderListViewControl()
    {
        InitializeComponent();

        DataContext = new ReadingOrderListViewModel(ListProvider, RoProvider);

        NavigateCommand = ReactiveCommand.Create<ReadingOrderOverview>(
            NavigateToReadingOrder,
            outputScheduler: AvaloniaScheduler.Instance);
    }

    public void NavigateToReadingOrder(ReadingOrderOverview overview)
    {
        var readingOrder = RoProvider.GetReadingOrder(overview);
        ArgumentNullException.ThrowIfNull(readingOrder);
        
        WeakReferenceMessenger.Default.Send(new NavigationMessage(new ReadingOrderViewControl(readingOrder)));
    }
}