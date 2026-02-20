namespace ROGraph.Backend.DataProviders.Interfaces;

public interface IReadingOrderDataSourceCreator
{
    /// <summary>
    /// Creates a data source
    /// </summary>
    /// <returns>A boolean indicating success</returns>
    public bool CreateDataSource();
}