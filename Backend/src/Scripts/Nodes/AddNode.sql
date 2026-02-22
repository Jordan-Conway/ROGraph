INSERT INTO Nodes (id, name, description, isCompleted, checkListId, origin, type)
VALUES (@nodeId, @name, @description, @isCompleted, @checkListId, @origin, @type)
ON CONFLICT (id) DO UPDATE
SET name = @name,
    description = @description,
    isCompleted = @isCompleted,
    checkListId = @checkListId,
    type = @type;

INSERT INTO ReadingOrders_Nodes (readingOrderId, nodeId, x, y)
VALUES (@readingOrderId, @nodeId, @x, @y)
ON CONFLICT (readingOrderId, nodeId) DO UPDATE 
SET x = @x,
    y = @y;

