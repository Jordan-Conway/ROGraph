using ROGraph.Shared.Models;

namespace ROGraph.Backend.DatabaseModels;

internal static class ReadingOrderOverviewExtensions
{
    public static ReadingOrderOverviewDbModel ToDbModel(this ReadingOrderOverview readingOrderOverview)
    {
        return new ReadingOrderOverviewDbModel(
            readingOrderOverview.Name,
            readingOrderOverview.Id,
            readingOrderOverview.Description,
            readingOrderOverview.MaxX,
            readingOrderOverview.MaxY
            );
    }

    public static ReadingOrderOverview ToOverview(this ReadingOrderOverviewDbModel dbModel)
    {
        return new ReadingOrderOverview(
            dbModel.Name,
            dbModel.Id,
            dbModel.Description,
            dbModel.MaxX,
            dbModel.MaxY
        );
    }
}