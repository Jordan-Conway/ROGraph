using System.Threading.Tasks;
using ROGraph.UI.Dialogs;

namespace ROGraph.UI.Services;

internal interface IDialogService
{
    Task<T> Open<T>(Dialog<T> dialog);

    Task<bool> OpenConfirmDialog(string message);
}