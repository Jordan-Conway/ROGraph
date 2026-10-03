using ROGraph.Shared.Models;

namespace ROGraph.Shared.Extensions;

public static class ReadingOrderExtensions
{
    public static ReadingOrderOverview ToOverview(this ReadingOrder readingOrder)
    {
        return new ReadingOrderOverview(
            readingOrder.Name,
            readingOrder.Id,
            readingOrder.Description,
            readingOrder.CoordinateTranslator?.GetNumberOfRows() ?? 0,
            readingOrder.CoordinateTranslator?.GetNumberOfColumns() ?? 0
        );
    }
}
