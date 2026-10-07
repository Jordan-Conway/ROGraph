using System;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.DatabaseModels;

internal static class ConnectorExtensions
{
    extension(CoordinateTranslator coordinateTranslator)
    {
        public Connector ToConnector(ConnectorDbModel model)
        {
            var x1 = coordinateTranslator.GetXFromInt(model.X1);
            var y1 = coordinateTranslator.GetYFromInt(model.Y1);
            var x2 = coordinateTranslator.GetXFromInt(model.X2);
            var y2 = coordinateTranslator.GetYFromInt(model.Y2);
            return new Connector((x1, y1), (x2, y2))
            {
                Id = model.Id
            };
        }

        public ConnectorDbModel ToConnectorDbModel(Connector connector, Guid readingOrderId)
        {
            var origin = coordinateTranslator.Translate(connector.Origin);
            var destination = coordinateTranslator.Translate(connector.Destination);
            return new ConnectorDbModel
            {
                Id = connector.Id,
                ReadingOrderId = readingOrderId,
                X1 = origin.Item1,
                Y1 = origin.Item2,
                X2 = destination.Item1,
                Y2 = destination.Item2,
            };
        }
    }
}
