using System;
using System.Reactive;
using Avalonia.Controls;
using ROGraph.UI.Views.ReadingOrderView;
using ReactiveUI;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.Backend.DataProviders.SQLiteProviders;
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
    
    public void NavigateToReadingOrder(Guid id)
    {
        var readingOrder = RoProvider.GetReadingOrder(id);
        ArgumentNullException.ThrowIfNull(readingOrder);
        
        WeakReferenceMessenger.Default.Send(new NavigationMessage(new ReadingOrderViewControl(readingOrder)));
    }
}