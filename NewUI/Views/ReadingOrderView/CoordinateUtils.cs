using System;

namespace ROGraph.NewUI.Views.ReadingOrderView;

internal static class CoordinateUtils
{
    private static readonly int IMAGE_SIZE = 240;
    private static readonly int IMAGE_GAP_SIZE = 80;

    /// <summary>
    /// Converts a node's column and row position to pixel positions
    /// </summary>
    /// <param name="position">A tuple representing the nodes column and row coordinates</param>
    /// <returns></returns>
    public static (int, int) GetNodePosition((int, int) position)
    {
        int x = position.Item1;
        int y = position.Item2;

        x = (x * IMAGE_SIZE) + (x * IMAGE_GAP_SIZE) + (IMAGE_GAP_SIZE / 2);
        y = (y * IMAGE_SIZE) + (y * (IMAGE_GAP_SIZE / 2)) + (IMAGE_GAP_SIZE / 4);

        return (x, y);
    }

    /// <summary>
    /// Gets the closest valid position for a given position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static (int, int) GetNearestValidNodePosition((double, double) position)
    {
        int x = (int)Math.Round(position.Item1, 0);
        int y = (int)Math.Round(position.Item2, 0);

        var coordinates = GetNodeCoordinates((x, y));

        return GetNodePosition(coordinates);
    }

    /// <summary>
    /// Gets the column and row of a node from it's position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static (int, int) GetNodeCoordinates((int, int) position)
    {
        int x = position.Item1;
        int y = position.Item2;

        x -= IMAGE_GAP_SIZE / 2;
        x /= IMAGE_SIZE + IMAGE_GAP_SIZE;

        y -= IMAGE_GAP_SIZE / 4;
        y /= IMAGE_SIZE + (IMAGE_GAP_SIZE / 2);

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
        int x1 = origin.Item1;
        int y1 = origin.Item2;
        int x2 = destination.Item1;
        int y2 = destination.Item2;

        x1 = (x1 * IMAGE_SIZE) + (x1 * IMAGE_GAP_SIZE) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 2);
        y1 = (y1 * IMAGE_SIZE) + (y1 * (IMAGE_GAP_SIZE / 2)) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 4);
        x2 = (x2 * IMAGE_SIZE) + (x2 * IMAGE_GAP_SIZE) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 2);
        y2 = (y2 * IMAGE_SIZE) + (y2 * (IMAGE_GAP_SIZE / 2)) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 4);

        return ((x1, y1), (x2, y2));
    }

    /// <summary>
    /// Gets the column number from an x position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static int GetColumnPosition(double position)
    {
        int index = (int)Math.Round(position, 0);
        index -= IMAGE_GAP_SIZE / 2;
        index /= IMAGE_SIZE + IMAGE_GAP_SIZE;

        return index;
    }

    /// <summary>
    /// Gets the row number from a y position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static int GetRowPosition(double position)
    {
        int index = (int)Math.Round(position, 0);
        index -= IMAGE_GAP_SIZE / 4;
        index /= IMAGE_SIZE + (IMAGE_GAP_SIZE / 2);

        return index;
    }
}