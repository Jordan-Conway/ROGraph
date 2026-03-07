UPDATE ReadingOrders
SET name = @name,
    description = @description
WHERE id = @id