using System;

namespace ROGraph.UI.Views.ReadingOrderView;

internal static class CoordinateUtils
{
    /// <summary>
    /// Converts a node's column and row position to pixel positions
    /// </summary>
    /// <param name="coordinates">A tuple representing the nodes column and row coordinates</param>
    /// <returns></returns>
    public static (int, int) GetNodePosition((int, int) coordinates)
    {
        var x = coordinates.Item1;
        var y = coordinates.Item2;
        var numHorizontalGaps = Math.Max(x -1, 0);
        var numVerticalGaps = Math.Max(y -1, 0);
        
        x = (x * Sizes.ImageSize) + (numHorizontalGaps * Sizes.ImageHorizontalGapSize);
        y = (y * Sizes.ImageSize) + (numVerticalGaps * Sizes.ImageVerticalGapSize);

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

        var verticalNodes = 0;
        var horizontalNodes = 0;

        while (y > 0)
        {
            y -= Sizes.ImageSize;
            verticalNodes++;
            
            if (y <= 0) continue;

            y -= Sizes.ImageVerticalGapSize;
        }
        
        while (x > 0)
        {
            x -= Sizes.ImageSize;
            horizontalNodes++;

            if (x <= 0) continue;
            
            x -= Sizes.ImageHorizontalGapSize;
        }

        return (horizontalNodes, verticalNodes);
    }

    /// <summary>
    /// Converts and connector's origin and destination coordinates to pixel positions
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static ((int, int), (int, int)) GetConnectorPositions((int, int) origin, (int, int) destination)
    {
        const int halfImageGapSize = Sizes.ImageSize / 2;

        var (x1, y1) = GetNodePosition(origin);
        var (x2, y2) = GetNodePosition(destination);

        x1 += halfImageGapSize;
        y1 += halfImageGapSize;
        x2 += halfImageGapSize;
        y2 += halfImageGapSize;

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
        index -= Sizes.ImageHorizontalGapSize / 2;
        index /= Sizes.ImageSize + Sizes.ImageHorizontalGapSize;

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
        index -= Sizes.ImageHorizontalGapSize / 4;
        index /= Sizes.ImageSize + (Sizes.ImageHorizontalGapSize / 2);

        return index;
    }
}