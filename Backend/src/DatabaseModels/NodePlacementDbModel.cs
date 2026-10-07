using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ROGraph.Backend.DatabaseModels;

[Table("ReadingOrders_Nodes")]
internal record NodePlacementDbModel
{
    public Guid Id { get; set; }
    public Guid ReadingOrderId  { get; set; }
    public Guid NodeId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}