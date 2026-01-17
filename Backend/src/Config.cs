namespace ROGraph.Backend;

public class Config
{
    public readonly Version DbVersion;

    internal Config(Version dbVersion)
    {
        this.DbVersion = dbVersion;
    }
}