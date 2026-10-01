using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ROGraph.Backend.Scripts;

internal static class ScriptReader
{
    private const string CreateDatabaseScriptName = "CreateDatabase.sql";
    private const string GetReadingOrderNodesScriptName = "GetReadingOrderNodes.sql";

    public static string GetCreateDatabaseScript()
    {
        return ReadResource(CreateDatabaseScriptName);
    }

    public static string GetReadingOrderNodesScript()
    {
        return ReadResource(GetReadingOrderNodesScriptName);
    }

    private static string ReadResource(string fileName)
    {
        var assembly = Assembly.GetAssembly(typeof(ScriptReader)) ?? throw new InvalidOperationException("Cannot find backend assembly");
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