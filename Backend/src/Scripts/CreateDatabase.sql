PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS ReadingOrders (
    id TEXT PRIMARY KEY,
    name TEXT,
    description TEXT,
    maxX INTEGER,
    maxY INTEGER,
    created INTEGER,
    lastModified INTEGER
);

CREATE TRIGGER IF NOT EXISTS UpdateReadingOrdersLastModified
    AFTER UPDATE OF id, name, description, maxX, maxY ON ReadingOrders
    FOR EACH ROW
BEGIN
    UPDATE ReadingOrders SET lastModified = CURRENT_TIMESTAMP
    WHERE id = old.id;
END;

CREATE TRIGGER IF NOT EXISTS SetReadingOrdersTimestamps
    AFTER INSERT ON ReadingOrders
    FOR EACH ROW
BEGIN
    UPDATE ReadingOrders SET created = CURRENT_TIMESTAMP, lastModified = CURRENT_TIMESTAMP
    WHERE id = new.id;
END;

CREATE TABLE IF NOT EXISTS Nodes (
    id TEXT PRIMARY KEY,
    name TEXT,
    description TEXT,
    isCompleted INT,
    checkListId TEXT,
    origin TEXT,
    type INT,
    created INTEGER,
    lastModified INTEGER
);

CREATE TRIGGER IF NOT EXISTS UpdateNodesLastModified
    AFTER UPDATE OF id, name, description, isCompleted, checkListId, origin, type ON Nodes
    FOR EACH ROW
BEGIN
    UPDATE Nodes SET lastModified = CURRENT_TIMESTAMP
    WHERE id = old.id;
END;

CREATE TRIGGER IF NOT EXISTS SetNodesTimestamps
    AFTER INSERT ON Nodes
    FOR EACH ROW
BEGIN
    UPDATE Nodes SET created = CURRENT_TIMESTAMP, lastModified = CURRENT_TIMESTAMP
    WHERE id = new.id;
END;

CREATE TABLE IF NOT EXISTS ReadingOrders_Nodes (
    readingOrderId TEXT NOT NULL,
    nodeId TEXT NOT NULL,
    
    FOREIGN KEY (readingOrderId)
        REFERENCES ReadingOrders (id),
    FOREIGN KEY (nodeId)
        REFERENCES Nodes (id)
);

CREATE UNIQUE INDEX IF NOT EXISTS ReadingOrders_Nodes_ROId_Index ON ReadingOrders_Nodes (readingOrderId);

CREATE TABLE IF NOT EXISTS Connectors (
    x1 INTEGER,
    y1 INTEGER,
    x2 INTEGER,
    y2 INTEGER,
    readingOrderId TEXT,
    
    FOREIGN KEY (readingOrderId)
        REFERENCES ReadingOrders (id)
);

CREATE UNIQUE INDEX IF NOT EXISTS Connectors_ReadingOrder_Index ON Connectors (readingOrderId);