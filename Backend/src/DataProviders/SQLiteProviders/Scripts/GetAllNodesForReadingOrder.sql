-- Ideally this would be a view or function but I couldn't work out how to do that with a parameter
SELECT * 
FROM nodes n
JOIN nodes_readingOrders nr
ON n.id = nr.nodeId
WHERE nr.nodeId = @id;
