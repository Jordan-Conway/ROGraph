CREATE TABLE IF NOT EXISTS nodes (
    id TEXT PRIMARY KEY NOT NULL,
    name TEXT,
    description TEXT,
    isCompleted INTEGER,
    origin TEXT,
    nodeType INTEGER,
    created INTEGER,
    lastModified INTEGER
);

-- Prevent created from changing on update
CREATE TRIGGER IF NOT EXISTS prevent_node_created_changing BEFORE UPDATE
ON nodes
WHEN old.created IS NOT NULL AND old.created <> new.created
BEGIN
    SELECT RAISE(ABORT, 'Created time of a node cannot be changed');
END;

-- Add last modified for new nodes
-- May not be needed due to update_nodes_last_modified
CREATE TRIGGER IF NOT EXISTS add_nodes_created_and_last_modified AFTER INSERT
ON nodes
BEGIN
    UPDATE nodes
    SET lastModified = current_timestamp
    WHERE id = new.id;
END;

-- Update last modified when nodes change
CREATE TRIGGER IF NOT EXISTS update_nodes_last_modified AFTER UPDATE
ON nodes
WHEN old.lastModified = new.lastModified
BEGIN
    UPDATE nodes
    SET lastModified = current_timestamp
    WHERE id = new.id;
END;

CREATE TABLE IF NOT EXISTS readingOrders (
    id TEXT PRIMARY KEY NOT NULL,
    name TEXT,
    description TEXT,
    created INTEGER,
    lastModified INTEGER
);

CREATE TABLE IF NOT EXISTS nodes_readingOrders (
    nodeId TEXT NOT NULL,
    readingOrderId TEXT NOT NULL,
    x INTEGER,
    y INTEGER,
    FOREIGN KEY (nodeId) REFERENCES nodes(id),
    FOREIGN KEY (readingOrderId) REFERENCES readingOrders(id)
);

CREATE TABLE IF NOT EXISTS connectors (
    id TEXT PRIMARY KEY NOT NULL,
    readingOrderId TEXT NOT NULL,
    x INTEGER,
    y INTEGER,
    FOREIGN KEY (readingOrderId) REFERENCES readingOrders(id)
);