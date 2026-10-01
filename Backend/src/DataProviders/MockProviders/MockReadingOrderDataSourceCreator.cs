using ROGraph.Backend.Contracts;

namespace ROGraph.Backend.DataProviders.MockProviders;

public class MockReadingOrderDataSourceCreator : IReadingOrderDataSourceCreator
{
    public bool CreateDataSource()
    {
        return true;
    }
}