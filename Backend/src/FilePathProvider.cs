namespace ROGraph.Backend;

internal static class FilePathProvider
{
    private const string ConfigFileName = "config.json";
    private const string DbFileName = "data.db";
    private static string _appDataDirectory = "";

    public static string GetConfigFilePath()
    {
        SetAppDataDirectory();
        return Path.Combine(_appDataDirectory, ConfigFileName);
    }

    public static string GetDatabaseFilePath()
    {
        SetAppDataDirectory();
        return Path.Combine(_appDataDirectory, DbFileName);
    }

    private static void SetAppDataDirectory()
    {
        _appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ROGraph");
    }
}
