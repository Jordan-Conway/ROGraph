using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.NewUI.Dialogs.EditNodeDialog;

internal class EditNodeDialogClosedMessage : ValueChangedMessage<bool>
{
    public EditNodeDialogClosedMessage(bool isCancelled) : base(isCancelled)
    {
        
    }
}