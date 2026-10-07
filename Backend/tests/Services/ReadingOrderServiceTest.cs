using Moq.AutoMock;
using ROGraph.Backend.Services;

namespace ROGraph.Backend.Tests.Services;

public class ReadingOrderServiceTest
{
    private readonly AutoMocker  _mocker = new();
    private ReadingOrderService _service;

    [SetUp]
    public void Setup()
    {
        _service = _mocker.CreateInstance<ReadingOrderService>();
    }
    
    
}