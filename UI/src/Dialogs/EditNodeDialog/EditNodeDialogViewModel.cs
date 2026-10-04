using System;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Messages;
using ROGraph.UI.ViewModels;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Dialogs.EditNodeDialog;

internal partial class EditNodeDialogViewModel : ViewModelBase
{
    private Node NodeModel {get; set;}
    public string Name {get; set;}
    public string? Description {get; set;}
    public bool IsCompleted {get; set;}

    public EditNodeDialogViewModel(Node node)
    {
        this.NodeModel = node;

        this.Name = node.Name;
        this.Description = node.Description;
        this.IsCompleted = node.IsCompleted;

        WeakReferenceMessenger.Default.Register<EditNodeDialogClosedMessage>(this, (r,m) =>
        {
            if(m.Value)
            {
                Console.WriteLine("No value");
                return;
            }

            node.Name = Name;
            node.Description = Description;
            node.IsCompleted = IsCompleted;

            WeakReferenceMessenger.Default.Send(new NodeEditedMessage(node));
        });
    }
}