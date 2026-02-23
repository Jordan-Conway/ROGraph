using System.Reflection;

namespace ROGraph.Backend.Scripts;

internal static class ScriptReader
{
    private const string CreateDatabaseScriptName = "CreateDatabase.sql";
    private const string GetAllReadingOrdersScriptName = "GetAllReadingOrders.sql";
    private const string GetReadingOrderScriptName = "GetReadingOrderScript.sql";
    private const string CreateReadingOrderScriptName = "CreateReadingOrder.sql";
    private const string GetReadingOrderNodesScriptName = "GetReadingOrderNodes.sql";
    private const string GetReadingOrderConnectorsScriptName = "GetReadingOrderConnectors.sql";
    private const string AddNodeScriptName = "AddNode.sql";
    private const string DeleteNodeScriptName = "DeleteNode.sql";
    private const string AddConnectorScriptName = "AddConnector.sql";
    private const string DeleteConnectorScriptName = "DeleteConnector.sql";

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

    public static string CreateReadingOrderScript()
    {
        return ReadResource(CreateReadingOrderScriptName);
    }

    public static string GetReadingOrderNodesScript()
    {
        return ReadResource(GetReadingOrderNodesScriptName);
    }

    public static string GetReadingOrderConnectorsScript()
    {
        return ReadResource(GetReadingOrderConnectorsScriptName);
    }

    public static string GetAddNodeScript()
    {
        return ReadResource(AddNodeScriptName);
    }

    public static string GetDeleteNodeScript()
    {
        return ReadResource(DeleteNodeScriptName);
    }

    public static string GetAddConnectorScript()
    {
        return ReadResource(AddConnectorScriptName);
    }

    public static string GetDeleteConnectorScript()
    {
        return ReadResource(DeleteConnectorScriptName);
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