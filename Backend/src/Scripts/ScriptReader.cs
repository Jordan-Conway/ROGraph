using System.Reflection;

namespace ROGraph.Backend.Scripts;

internal static class ScriptReader
{
    private const string CreateDatabaseScriptName = "CreateDatabase.sql";
    private const string GetAllReadingOrdersScriptName = "GetAllReadingOrders.sql";
    private const string GetReadingOrderScriptName = "GetReadingOrderScript.sql";
    private const string GetReadingOrderNodesScriptName = "GetReadingOrderNodes.sql";
    private const string GetReadingOrderConnectorsScriptName = "GetReadingOrderConnectors.sql";

    public static string GetCreateDatabaseScript()
    {
        return ReadResource(CreateDatabaseScriptName);
    }

    public static string GetAllReadingOrdersScript()
    {
        return ReadResource(GetAllReadingOrdersScriptName);
    }

    public static string GetReadingOrderScript()
    {
        return ReadResource(GetReadingOrderScriptName);
    }

    public static string GetReadingOrderNodesScript()
    {
        return ReadResource(GetReadingOrderNodesScriptName);
    }

    public static string GetReadingOrderConnectorsScript()
    {
        return ReadResource(GetReadingOrderConnectorsScriptName);
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