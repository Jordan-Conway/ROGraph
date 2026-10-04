using System;
using System.Reactive;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Dialogs.EditReadingOrderDialog;

public class EditReadingOrderDialogViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    private readonly Guid _id;
    private readonly int _maxX = 0;
    private readonly int _maxY = 0;
    
    public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelChangesCommand { get; }

    public EditReadingOrderDialogViewModel(ReadingOrderOverview readingOrderOverview)
    {
        Name = readingOrderOverview.Name;
        Description = readingOrderOverview.Description ?? string.Empty;
        _id = readingOrderOverview.Id;
        _maxX = readingOrderOverview.MaxX;
        _maxY = readingOrderOverview.MaxY;

        SaveChangesCommand = ReactiveCommand.Create(SaveChanges);

        CancelChangesCommand = ReactiveCommand.Create(CancelChanges);
    }

    private void SaveChanges()
    {
        var overview = new ReadingOrderOverview(Name, _id, Description, _maxX, _maxY);
        var message = new EditReadingOrderDialogClosedMessage(overview);
        WeakReferenceMessenger.Default.Send(message);
    }

    private void CancelChanges()
    {
        var message = new EditReadingOrderDialogClosedMessage();
        WeakReferenceMessenger.Default.Send(message);
    }
}