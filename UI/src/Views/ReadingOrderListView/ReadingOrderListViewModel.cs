using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;
using ROGraph.Backend.Contracts;
using ROGraph.Shared.Models;
using ROGraph.UI.Messages;
using ROGraph.UI.Pages;
using ROGraph.UI.Services;

namespace ROGraph.UI.Views.ReadingOrderListView;

public partial class ReadingOrderListViewModel : ObservableObject
{
    private readonly IMessagingService _messagingService;
    private readonly IMessenger _messenger;
    
    private ObservableCollection<ReadingOrderOverview> _overviews;

    public ObservableCollection<ReadingOrderOverview> Overviews
    {
        get => _overviews;
        set => SetProperty(ref _overviews, value);
    }
    
    public ReactiveCommand<Guid, Unit> EditReadingOrderCommand { get; set; }

    public ReadingOrderListViewModel(IMessagingService messagingService, IMessenger messenger)
    {
        ArgumentNullException.ThrowIfNull(messagingService);
        
        _messagingService = messagingService;
        _messenger = messenger;

        var existingOverviews = _messagingService.GetReadingOrderOverviews();
        _overviews = new ObservableCollection<ReadingOrderOverview>(existingOverviews.GetAwaiter().GetResult());
        Debug.WriteLine($"Resolved {_overviews.Count} overviews ");
        EditReadingOrderCommand = ReactiveCommand.CreateFromTask<Guid>(EditReadingOrder);
        EditReadingOrderCommand.ThrownExceptions.Subscribe(ex =>
        {
            Debug.WriteLine(ex);
        });
    }

    [RelayCommand]
    public void NavigateToReadingOrder(Guid readingOrderId)
    {
        _messenger.Send(new NavigationMessage(new PageType.ReadingOrderPage(readingOrderId)));
    }
    
    [RelayCommand]
    public async void CreateReadingOrder()
    {
        var overview = new ReadingOrderOverview("New", Guid.NewGuid());
        
        var created = await _messagingService.CreateReadingOrder(overview);

        if (!created)
        {
            Debug.WriteLine($"Failed to create reading order with id: {overview.Id}");
            return;
        }
        
        Overviews.Add(overview);
    }
    
    public async Task EditReadingOrder(Guid id)
    {
        var original = Overviews.FirstOrDefault(x => x.Id == id);
        
        if (original == null)
        {
            return;
        }
        
        var overview = await WeakReferenceMessenger.Default.Send(new EditReadingOrderMessage(original));

        if (overview == null)
        {
            return;
        }

        var updated = await _messagingService.UpdateReadingOrder(overview);

        if (!updated)
        {
            Debug.WriteLine($"Failed to update reading order with id: {id}");
            return;
        }
        
        Overviews.Replace(original, overview);
    }

    [RelayCommand]
    public async Task DeleteReadingOrder(Guid id)
    {
        var deleted = await _messagingService.DeleteReadingOrder(id);

        if (!deleted)
        {
            Debug.WriteLine($"Failed to delete reading order with id: {id}");
            return;
        }
        
        Overviews.Remove(Overviews.First(x => x.Id == id));
    }
    
    private async Task RefreshReadingOrders(CancellationToken token = default)
    {
        Overviews.Clear();

        var overviews = await _messagingService.GetReadingOrderOverviews(token);
        
        Overviews.AddRange(overviews);
    }
}