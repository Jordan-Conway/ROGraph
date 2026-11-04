using System;
using CommunityToolkit.Mvvm.Input;

namespace ROGraph.UI.Dialogs;

internal abstract partial class Dialog<TResult> : BaseDialog
{
    public event EventHandler<DialogClosedEventArgs<TResult>> CloseDialog;

    [RelayCommand]
    public virtual void Close(TResult result)
    {
        var eventArgs = new DialogClosedEventArgs<TResult>(result);
        CloseDialog.Invoke(this, eventArgs);
    }
}