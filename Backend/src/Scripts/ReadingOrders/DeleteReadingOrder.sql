UPDATE ReadingOrders
SET deleted = 1
WHERE id = @id;