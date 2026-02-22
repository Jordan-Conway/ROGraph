INSERT INTO Connectors (x1, y1, x2, y2, readingOrderId)
VALUES (@x1, @y1, @x2, @y2, @roId)
ON CONFLICT (x1, y1, x2, y2, readingOrderId) DO NOTHING;
