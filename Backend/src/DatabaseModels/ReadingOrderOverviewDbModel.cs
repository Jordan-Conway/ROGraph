using System;
using System.ComponentModel.DataAnnotations.Schema;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.DatabaseModels;

[Table("ReadingOrders")]
internal record ReadingOrderOverviewDbModel : ReadingOrderOverview
{
    [Column("status")]
    public ReadingOrderStatus Status { get; set; } = ReadingOrderStatus.Active;
    
    public ReadingOrderOverviewDbModel(string? name, Guid id, string? description = null, int maxX = 0, int maxY = 0) :
        base(name, id, description, maxX, maxY)
    {
        
    }
}