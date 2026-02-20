using System.Data.SQLite;
using Microsoft.Extensions.Logging;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.Backend.Scripts;

namespace ROGraph.Backend.DataProviders.SQLiteProviders;

internal class SqlDataSourceCreator : IReadingOrderDataSourceCreator
{
    private readonly ILogger<SqlDataSourceCreator> _logger;
    
    public SqlDataSourceCreator(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SqlDataSourceCreator>();
    }
    
    public bool CreateDataSource()
    {
        var dbFilePath = FilePathProvider.GetDatabaseFilePath();

        if (!File.Exists(dbFilePath))
        {
            _logger.LogInformation("Database {dbName} not found, creating from scratch", dbFilePath);
            File.Create(dbFilePath).Close();
        }

        using var connection = new SQLiteConnection("Data Source=" + dbFilePath);
        try
        {
            connection.Open();
            var createDbScript = ScriptReader.GetCreateDatabaseScript();
            using var command = connection.CreateCommand();
            command.CommandText = createDbScript;
            command.ExecuteNonQuery();
            _logger.LogInformation("Database {dbName} Created", dbFilePath);
        }
        catch (SQLiteException ex)
        {
            _logger.LogError(ex, "Failed to create database");
            return false;
        }

        return true;
    }
}