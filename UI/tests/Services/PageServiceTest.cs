using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;
using ROGraph.Shared.Models;
using ROGraph.UI.Pages;
using ROGraph.UI.Services;
using ROGraph.UI.Services.StateService;
using ROGraph.UI.Views;
using ROGraph.UI.Views.ReadingOrderView;

namespace ROGraph.UI.Tests.Services;

public sealed class PageServiceTest
{
    private readonly AutoMocker _mocker = new();
    private PageService _service;

    [SetUp]
    public void Setup()
    {
        _service = _mocker.CreateInstance<PageService>();
    }
    
    [Test]
    public void GetPage_ReadingOrderListPage_ReturnsPage()
    {
        // Arrange
        var listPage = _mocker.CreateInstance<ReadingOrderListView>();
        IList<ReadingOrderOverview> overviews =
        [
            new("Reading Order A", Guid.NewGuid()),
            new("Reading Order B", Guid.NewGuid()),
        ];
        var readingOrders = Task.FromResult(overviews);
        
        _mocker.Setup<IMessagingService, Task<IList<ReadingOrderOverview>>>(m => m.GetReadingOrderOverviews())
            .Returns(readingOrders);
        _mocker.Setup<IServiceProvider, object>(s => s.GetRequiredService(typeof(ReadingOrderListView)))
            .Returns(listPage);
        
        // Act
        var result = _service.GetPage(new PageType.ReadingOrderListPage());
        
        // Assert
        Assert.That(result, Is.InstanceOf<ReadingOrderListView>());
    }

    [Test]
    public void GetPage_ReadingOrderPage_SetsSelectedIdAndReturnsPage()
    {
        // Arrange
        var page =  _mocker.CreateInstance<ReadingOrderView>();
        var readingOrderId = Guid.NewGuid();
        var mockStateService = _mocker.GetMock<IStateService>();
        _mocker.Setup<IServiceProvider, object>(s => s.GetRequiredService(typeof(ReadingOrderView)))
            .Returns(page);

        // Act
        var result = _service.GetPage(new PageType.ReadingOrderPage(readingOrderId));
        
        // Assert
        Assert.That(result, Is.InstanceOf<ReadingOrderView>());
        mockStateService.VerifySet(s => s.SelectedReadingOrderId = readingOrderId, Times.Once);
    }
}