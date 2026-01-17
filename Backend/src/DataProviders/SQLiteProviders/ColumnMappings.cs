namespace ROGraph.Backend.DataProviders.SQLiteProviders;

internal enum NodesTableColumnMapping
{
    ID = 0,
    NAME = 1,
    DESCRIPTION = 2,
    IS_COMPLETED = 3,
    ORIGIN = 4,
    NODE_TYPE = 5,
    CREATED = 6,
    LAST_MODIFIED = 7
}

internal enum ReadingOrderTableColumnMapping
{
    ID = 0,
    NAME = 1,
    DESCRIPTION = 2,
    CREATED = 3,
    LAST_MODIFIED = 4
}

internal enum NodesReadingOrdersTableColumnMapping
{
    NODE_ID = 0,
    READING_ORDER_ID = 1,
    X = 2,
    Y = 3
}

internal enum ConnectorTableColumnMapping
{
    ID = 0,
    READING_ORDER_ID = 1,
    X = 2,
    Y = 3
}