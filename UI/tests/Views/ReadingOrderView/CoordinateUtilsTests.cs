using ROGraph.UI.Views.ReadingOrderView;

namespace ROGraph.UI.Tests.Views.ReadingOrderView;

public sealed class CoordinateUtilsTests
{
    [Test]
    public void TranslatesNodesBetweenPositionAndCoordinatesAndBack()
    {
        var initialPosition = (10, 4);
        
        var position = CoordinateUtils.GetNodePosition(initialPosition);

        var resultPosition = CoordinateUtils.GetNodeCoordinates(position);
        
        Assert.That(resultPosition, Is.EqualTo(initialPosition));
    }

    [Test]
    public void ShouldTranslateConnectorsHalfImageSizeMoreThanNodes()
    {
        // Arrange
        var node1Coordinates = (2, 1);
        var node2Coordinates = (3, 4);

        // Act
        var node1Position = CoordinateUtils.GetNodePosition(node1Coordinates);
        var node2Position = CoordinateUtils.GetNodePosition(node2Coordinates);
        var (connectorOrigin, connectorDestination) = CoordinateUtils.GetConnectorPositions(node1Coordinates, node2Coordinates);
        
        // Assert
        const int expectedDifference = Sizes.ImageSize / 2;
        Assert.Multiple(() =>
        {
            Assert.That(node1Position.Item1, Is.EqualTo(connectorOrigin.Item1 - expectedDifference));
            Assert.That(node1Position.Item2, Is.EqualTo(connectorOrigin.Item2 - expectedDifference));
            Assert.That(node2Position.Item1, Is.EqualTo(connectorDestination.Item1 - expectedDifference));
            Assert.That(node2Position.Item2, Is.EqualTo(connectorDestination.Item2 - expectedDifference));
        });
    }
    
    [TestCaseSource(nameof(NodePositionTestCases))]
    public void GetNodePosition_ReturnsCorrectPosition((int xCoordinate, int yCoordinate, int expectedXPosition, int expectedYPosition) testData)
    {
        // Arrange
        var initialCoordinate = (testData.xCoordinate, testData.yCoordinate);
        
        // Act
        var (xResult, yResult) = CoordinateUtils.GetNodePosition(initialCoordinate);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(xResult, Is.EqualTo(testData.expectedXPosition));
            Assert.That(yResult, Is.EqualTo(testData.expectedYPosition));
        });
    }

    [TestCaseSource(nameof(NearestValidNodePositionTestCases))]
    public void GetNearestValidNodePosition_ReturnsNearestValidPosition(
        (int clickX, int clickY, int expectedX, int expectedY) testData)
    {
        // Arrange
        var clickPosition = (testData.clickX, testData.clickY);
        
        // Act
        var (xResult, yResult) = CoordinateUtils.GetNearestValidNodePosition(clickPosition);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(xResult, Is.EqualTo(testData.expectedX));
            Assert.That(yResult, Is.EqualTo(testData.expectedY));
        });
    }

    [TestCaseSource(nameof(NodeCoordinatesTestCases))]
    public void GetNodeCoordinates((int xPosition, int yPosition, int expectedXCoordinate, int expectedYCoordinate) testData)
    {
        // Arrange
        var nodePosition = (testData.xPosition, testData.yPosition);
        
        // Act
        var (xResult, yResult) = CoordinateUtils.GetNodeCoordinates(nodePosition);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(xResult, Is.EqualTo(testData.expectedXCoordinate));
            Assert.That(yResult, Is.EqualTo(testData.expectedYCoordinate));
        });
    }

    [TestCaseSource(nameof(ConnectorPositionTestCases))]
    public void GetConnectorPositions_ReturnsCorrectPositions(((int, int) origin, (int, int) destination, (int, int) expectedOrigin, (int, int) expectedDestination) testData)
    {
        // Act
        var (originResult, destinationResult) = CoordinateUtils.GetConnectorPositions(testData.origin, testData.destination);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(originResult, Is.EqualTo(testData.expectedOrigin));
            Assert.That(destinationResult, Is.EqualTo(testData.expectedDestination));
        });
    }

    public static readonly IEnumerable<(int, int, int, int)> NodePositionTestCases =
    [
        (0, 0, 0, 0),
        (0, 1, 0, 180),
        (1, 1, 180, 180),
        (2, 2, 480, 450),
        (3, 7, 780, 1800)
    ];

    public static readonly IEnumerable<(int, int, int, int)> NearestValidNodePositionTestCases =
    [

    ];

    public static readonly IEnumerable<(int, int, int, int)> NodeCoordinatesTestCases =
    [
        (0, 0, 0, 0),
        (0, 180, 0, 1),
        (180, 180, 1, 1),
        (480, 450, 2, 2),
        (780, 1800, 3, 7)
    ];

    public static readonly IEnumerable<((int, int), (int, int), (int, int), (int, int))> ConnectorPositionTestCases =
    [
        ((2, 1), (3, 4), (570, 270), (870, 1080)),
        ((0, 0), (1, 0), (90, 90), (270, 90))
    ];
}