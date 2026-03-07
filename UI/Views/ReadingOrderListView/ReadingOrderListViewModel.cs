using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.Shared.Models;
using ROGraph.UI.Messages;

namespace ROGraph.UI.Views.ReadingOrderListView;

internal partial class ReadingOrderListViewModel : ObservableObject
{
    private readonly IReadingOrderProvider _readingOrderProvider;
    
    private ObservableCollection<ReadingOrderOverview> _overviews;

    public ObservableCollection<ReadingOrderOverview> Overviews
    {
        get => _overviews;
        set => SetProperty(ref _overviews, value);
    }
    
    public ReactiveCommand<Guid, Unit> EditReadingOrderCommand { get; set; }

    public ReadingOrderListViewModel(IReadingOrderProvider roProvider)
    {
        ArgumentNullException.ThrowIfNull(roProvider);
        Debug.WriteLine(roProvider.GetType());
        
        _readingOrderProvider = roProvider;
        _overviews = new ObservableCollection<ReadingOrderOverview>(_readingOrderProvider.GetReadingOrders());
        EditReadingOrderCommand = ReactiveCommand.CreateFromTask<Guid>(EditReadingOrder);
        EditReadingOrderCommand.ThrownExceptions.Subscribe(ex =>
        {
            Debug.WriteLine("ENCOUNTERED ERROR ON EDIT");
            Debug.WriteLine(ex);
        });
        RegisterMessages();
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
        
        Overviews.Replace(original, overview);
        var updated = _readingOrderProvider.UpdateReadingOrderOverview(overview);
        Debug.WriteLine(updated);
    }

    [RelayCommand]
    public void DeleteReadingOrder(Guid id)
    {
        _readingOrderProvider.DeleteReadingOrder(id);
        Overviews.Remove(Overviews.First(x => x.Id == id));
    }
    
    private void RefreshReadingOrders()
    {
        Overviews.Clear();
        
        _overviews.AddRange(_readingOrderProvider.GetReadingOrders());
    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<ReadingOrderAddedMessage>(this, (r,m) =>
        {
            _readingOrderProvider.CreateReadingOrder(m.Value);
            RefreshReadingOrders();
        });
    }
}