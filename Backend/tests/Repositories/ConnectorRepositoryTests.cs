using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ROGraph.Backend.Context;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Backend.Repositories.Connectors;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Tests.Repositories;

public class ConnectorRepositoryTests : RepositoryTest
{
    private ConnectorRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = Mocker.CreateInstance<ConnectorRepository>();
    }

    [Test]
    public async Task GetConnectorsForReadingOrder_ReturnsConnectorsForReadingOrder()
    {
        // Arrange
        var translator = new CoordinateTranslator(2, 2);
        
        // Act
        var result = await _repository.GetConnectorsForReadingOrder(ExistingReadingOrderId, translator,
            TestContext.CurrentContext.CancellationToken);
        
        // Assert
        var expected = new List<Connector>
        {
            new(translator.Translate((0, 0)), translator.Translate((1, 0)))
            {
                Id = ExistingConnectorId1
            },
            new(translator.Translate((1, 0)), translator.Translate((2, 2)))
            {
                Id = ExistingConnectorId2
            },
        };
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task CreateConnector_ConnectorIsCreated()
    {
        // Arrange
        var translator = new CoordinateTranslator(2, 2);
        var origin = (translator.GetXFromInt(1),  translator.GetYFromInt(1));
        var destination = (translator.GetXFromInt(2), translator.GetYFromInt(2));
        var connector = new Connector(origin, destination)
        {
            Id = Guid.NewGuid()
        };
        
        // Act
        await _repository.CreateConnector(connector, translator, TestContext.CurrentContext.CancellationToken);
        
        // Assert
        var expectedOrigin = translator.Translate(origin);
        var expectedDestination = translator.Translate(destination);
        var expected = new ConnectorDbModel
        {
            Id = connector.Id,
            X1 = expectedOrigin.Item1,
            Y1 = expectedOrigin.Item2,
            X2 = expectedDestination.Item1,
            Y2 = expectedDestination.Item2,
        };
        Mocker.Verify<IDBContext>(c => c.Add(expected, TestContext.CurrentContext.CancellationToken));
    }
    
    [Test]
    public async Task CreateConnector_ConnectorHasNoId_IdIsSet()
    {
        // Arrange
        var translator = new CoordinateTranslator(2, 2);
        var origin = (translator.GetXFromInt(1),  translator.GetYFromInt(1));
        var destination = (translator.GetXFromInt(2), translator.GetYFromInt(2));
        var connector = new Connector(origin, destination)
        {
            Id = Guid.NewGuid()
        };
        
        // Act
        await _repository.CreateConnector(connector, translator, TestContext.CurrentContext.CancellationToken);
        
        // Assert
        Mocker.Verify<IDBContext>(c => c.Add(It.Is<ConnectorDbModel>(model => model.Id != Guid.Empty), TestContext.CurrentContext.CancellationToken ));
    }
}