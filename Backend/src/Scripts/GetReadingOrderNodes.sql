SELECT * FROM Nodes n JOIN ReadingOrders_Nodes ron
ON n.id = ron.nodeId
WHERE readingOrderId = @roId;