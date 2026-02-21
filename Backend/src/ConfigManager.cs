using System.Reflection;
using System.Text.Json;

namespace ROGraph.Backend;

public static class ConfigManager
{
    private static Config? _config;
    private static readonly object Padlock = new object();

    public static Config Instance
    {
        get
        {
            lock(Padlock)
            {
                return _config ?? LoadConfig();
            }
        }
    }

    private static Config LoadConfig()
    {
        var filePath = FilePathProvider.GetConfigFilePath();

        if(File.Exists(filePath))
        {
            var data = File.ReadAllText(filePath);
            try
            {
                _config = JsonSerializer.Deserialize<Config>(data);
            }
            catch(JsonException)
            {
                Console.Error.WriteLine($"Failed to deserialize config. Config file at {filePath}. The program will now exit");
                Environment.Exit(1);
            }
        }
        else
        {
            Console.WriteLine("Config file not found, loading default configuration. If you have previously used this application, this may cause data corruption");
            _config = CreateDefaultConfig();
        }
        
        return _config ?? throw new InvalidOperationException("Failed to load configuration");
    }

    private static Config CreateDefaultConfig()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version ?? throw new InvalidProgramException("Version number not found");

        return new Config(version);
    }
}