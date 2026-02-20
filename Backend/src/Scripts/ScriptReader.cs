using System.Reflection;

namespace ROGraph.Backend.Scripts;

internal static class ScriptReader
{
    private const string CreateDatabaseScriptName = "CreateDatabase.sql";

    public static string GetCreateDatabaseScript()
    {
        var assembly = Assembly.GetAssembly(typeof(ScriptReader)) ?? throw new InvalidOperationException("Cannot find backend assembly");
        return ReadResource(CreateDatabaseScriptName, assembly);
    }

    private static string ReadResource(string fileName, Assembly assembly)
    {
        var resourceName = GetResourceName(fileName, assembly);
        using var stream = assembly.GetManifestResourceStream(resourceName) ?? 
                           throw new InvalidOperationException($"Cannot load resource stream {fileName}");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
    
    private static string GetResourceName(string fileName, Assembly assembly)
    {
        return 
            assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(fileName)) ??
            throw new InvalidOperationException($"Cannot find resource {fileName}");
    }
}