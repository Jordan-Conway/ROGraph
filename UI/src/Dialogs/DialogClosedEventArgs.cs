using System;

namespace ROGraph.UI.Dialogs;

internal class DialogClosedEventArgs<T>(T result) : EventArgs
{
    public T Result { get; } = result;
}