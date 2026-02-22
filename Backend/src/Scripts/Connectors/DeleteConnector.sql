DELETE FROM Connectors
WHERE readingOrderId = @roId AND
      x1 = @x1 AND
      y1 = @y1 AND
      x2 = @x2 AND
      y2 = @y2;