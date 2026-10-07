using ROGraph.Backend.Contracts;

namespace ROGraph.Backend.DataProviders;

public class MockReadingOrderDataSourceCreator : IReadingOrderDataSourceCreator
{
    public bool CreateDataSource()
    {
        return true;
    }
}