using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ROGraph.UI.Controls;
using ROGraph.UI.Dialogs;

namespace ROGraph.UI.Services;

internal class DialogService : IDialogService
{
    private readonly IDialogContainer _dialogContainer;

    public DialogService([FromKeyedServices("dialogContainer")] IDialogContainer dialogContainer)
    {
        _dialogContainer = dialogContainer;
    }
    
    public Task<TResult> Open<TResult>(Dialog<TResult> dialog)
    {
        // For now, only one dialog can be open. At some point, we may add support for opening dialogs on top of each other.
        if (_dialogContainer.DialogControl != null)
        {
            throw new InvalidOperationException("Cannot open a dialog while another is already open");
        }
        
        var tcs = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        
        dialog.CloseDialog += OnCloseDialog;
        _dialogContainer.DialogControl = dialog;
        Debug.WriteLine("Dialog opened");

        return tcs.Task;
        
        void OnCloseDialog(object? sender, DialogClosedEventArgs<TResult> args)
        {
            Debug.WriteLine("Closing dialog");
            dialog.CloseDialog -= OnCloseDialog;
            _dialogContainer.DialogControl = null;
            tcs.TrySetResult(args.Result);
        }
    }
}