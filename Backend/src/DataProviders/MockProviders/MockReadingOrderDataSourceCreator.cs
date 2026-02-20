using ROGraph.Backend.DataProviders.Interfaces;

namespace ROGraph.Backend.DataProviders.MockProviders;

public class MockReadingOrderDataSourceCreator : IReadingOrderDataSourceCreator
{
    public bool CreateDataSource()
    {
        return true;
    }
}