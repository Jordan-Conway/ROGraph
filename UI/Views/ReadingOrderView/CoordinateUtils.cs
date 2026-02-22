using System;

namespace ROGraph.UI.Views.ReadingOrderView;

internal static class CoordinateUtils
{
    private const int ImageSize = 240;
    private const int ImageGapSize = 80;

    /// <summary>
    /// Converts a node's column and row position to pixel positions
    /// </summary>
    /// <param name="position">A tuple representing the nodes column and row coordinates</param>
    /// <returns></returns>
    public static (int, int) GetNodePosition((int, int) position)
    {
        var x = position.Item1;
        var y = position.Item2;

        x = (x * ImageSize) + (x * ImageGapSize) + (ImageGapSize / 2);
        y = (y * ImageSize) + (y * (ImageGapSize / 2)) + (ImageGapSize / 4);

        return (x, y);
    }

    /// <summary>
    /// Gets the closest valid position for a given position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static (int, int) GetNearestValidNodePosition((double, double) position)
    {
        var x = (int)Math.Round(position.Item1, 0);
        var y = (int)Math.Round(position.Item2, 0);

        var coordinates = GetNodeCoordinates((x, y));

        return GetNodePosition(coordinates);
    }

    /// <summary>
    /// Gets the column and row of a node from its position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static (int, int) GetNodeCoordinates((int, int) position)
    {
        var x = position.Item1;
        var y = position.Item2;

        x -= ImageGapSize / 2;
        x /= ImageSize + ImageGapSize;

        y -= ImageGapSize / 4;
        y /= ImageSize + (ImageGapSize / 2);

        return (x, y);
    }

    /// <summary>
    /// Converts and connector's origin and destination coordinates to pixel positions
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static ((int, int), (int, int)) GetConnectorPositions((int, int) origin, (int, int) destination)
    {
        var x1 = origin.Item1;
        var y1 = origin.Item2;
        var x2 = destination.Item1;
        var y2 = destination.Item2;

        x1 = (x1 * ImageSize) + (x1 * ImageGapSize) + (ImageSize / 2) + (ImageGapSize / 2);
        y1 = (y1 * ImageSize) + (y1 * (ImageGapSize / 2)) + (ImageSize / 2) + (ImageGapSize / 4);
        x2 = (x2 * ImageSize) + (x2 * ImageGapSize) + (ImageSize / 2) + (ImageGapSize / 2);
        y2 = (y2 * ImageSize) + (y2 * (ImageGapSize / 2)) + (ImageSize / 2) + (ImageGapSize / 4);

        return ((x1, y1), (x2, y2));
    }

    /// <summary>
    /// Gets the column number from an x position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static int GetColumnPosition(double position)
    {
        var index = (int)Math.Round(position, 0);
        index -= ImageGapSize / 2;
        index /= ImageSize + ImageGapSize;

        return index;
    }

    /// <summary>
    /// Gets the row number from a y position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static int GetRowPosition(double position)
    {
        var index = (int)Math.Round(position, 0);
        index -= ImageGapSize / 4;
        index /= ImageSize + (ImageGapSize / 2);

        return index;
    }
}