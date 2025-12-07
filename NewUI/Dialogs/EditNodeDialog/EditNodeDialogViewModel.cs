using System;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.NewUI.Messages;
using ROGraph.NewUI.Models;
using ROGraph.NewUI.ViewModels;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Dialogs.EditNodeDialog;

internal partial class EditNodeDialogViewModel : ViewModelBase
{
    private NodeModel _nodeModel {get; set;}
    public string Name {get; set;} = "";
    public string? Description {get; set;} = "";
    public bool IsCompleted {get; set;} = false;

    public EditNodeDialogViewModel(NodeModel nodeModel)
    {
        this._nodeModel = nodeModel;

        this.Name = nodeModel.Node.Name;
        this.Description = nodeModel.Node.Description;
        this.IsCompleted = nodeModel.Node.IsCompleted;

        WeakReferenceMessenger.Default.Register<EditNodeDialogClosedMessage>(this, (r,m) =>
        {
            if(m.Value)
            {
                Console.WriteLine("No value");
                return;
            }

            Node node = _nodeModel.Node;
            node.Name = Name;
            node.Description = Description;
            node.IsCompleted = IsCompleted;

            WeakReferenceMessenger.Default.Send(new NodeEditedMessage(node));
        });
    }
}