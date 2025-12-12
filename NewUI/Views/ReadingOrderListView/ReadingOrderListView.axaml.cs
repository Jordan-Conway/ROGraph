using System;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ROGraph.Backend.Data.DataProviders.Interfaces;
using ROGraph.Backend.Data.DataProviders.MockProviders;
using ROGraph.NewUI.Views.ReadingOrderView;
using ROGraph.Shared.Models;
using ReactiveUI;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.NewUI.Messages;

namespace ROGraph.NewUI.Views.ReadingOrderListView;

internal partial class ReadingOrderListViewControl : UserControl
{
    private static readonly IReadingOrderListProvider listProvider = new MockReadingOrderListProvider();
    private static readonly IReadingOrderProvider roProvider = new MockReadingOrderProvider();

    public ReactiveCommand<ReadingOrderOverview, Unit> NavigateCommand { get; }

    public ReadingOrderListViewControl()
    {
        InitializeComponent();

        DataContext = new ReadingOrderListViewModel(listProvider, roProvider);

        NavigateCommand = ReactiveCommand.Create<ReadingOrderOverview>(
            NavigateToReadingOrder,
            outputScheduler: AvaloniaScheduler.Instance);
    }

    public void NavigateToReadingOrder(ReadingOrderOverview overview)
    {
        ReadingOrder? readingOrder = roProvider.GetReadingOrder(overview);
        ArgumentNullException.ThrowIfNull(readingOrder);
        
        WeakReferenceMessenger.Default.Send(new NavigationMessage(new ReadingOrderViewControl(readingOrder)));
    }
}