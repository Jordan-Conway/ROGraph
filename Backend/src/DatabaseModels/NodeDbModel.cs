using System;
using System.ComponentModel.DataAnnotations.Schema;
using ROGraph.Shared.Enums;

namespace ROGraph.Backend.DatabaseModels;

[Table("Nodes")]
internal record NodeDbModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }  = string.Empty;
    public bool IsCompleted  { get; set; }
    public Guid? ChecklistId { get; set; }
    public Guid Origin { get; set; }
    public NodeType Type { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}