using ROGraph.Shared.Models.Checklist;
using ROGraph.UI.ViewModels;

namespace ROGraph.UI.Dialogs.ChecklistDialog;

public class ChecklistDialogViewModel : ViewModelBase
{
    public Checklist Checklist { get; set; }

    public ChecklistDialogViewModel(Checklist checklist)
    {
        Checklist = checklist;
    }
}