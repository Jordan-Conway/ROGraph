using ROGraph.Shared.Models;

namespace ROGraph.UI.Dialogs.EditReadingOrderDialog;

public class EditReadingOrderDialogClosedMessage
{
    public ReadingOrderOverview? ReadingOrderOverview { get; set; }

    public EditReadingOrderDialogClosedMessage(ReadingOrderOverview? readingOrderOverview = null)
    {
        ReadingOrderOverview = readingOrderOverview ?? null;
    }
}