using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROGraph.Backend.DatabaseModels;

[Table("Connectors")]
internal record ConnectorDbModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public int X1 { get; set; } = 0;
    public int Y1 { get; set; }  = 0;
    public int X2 { get; set; }  = 0;
    public int Y2 { get; set; }  = 0;
    public Guid ReadingOrderId { get; set; }  = Guid.Empty;
}