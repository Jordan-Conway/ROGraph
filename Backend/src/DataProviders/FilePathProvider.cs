namespace ROGraph.Backend.DataProviders;

public class FilePathProvider
{
    private const string ConfigFileName = "config.json";
    private const string DbFileName = "data.db";

    public static string GetConfigFilePath()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        return Path.Combine(appDataPath, ConfigFileName);
    }

    public static string GetDatabaseFilePath()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        return Path.Combine(appDataPath, DbFileName);
    }  
}
