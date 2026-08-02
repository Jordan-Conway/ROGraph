namespace ROGraph.Shared.Models.Checklist;

public class Checklist
{
    public IList<ChecklistHeader> Headers { get; } = [];
    public IList<ChecklistRow> Rows { get; } = [];

    public void AddHeader(ChecklistHeader header)
    {
        Headers.Add(header);
        
        foreach (var checklistRow in Rows)
        {
            checklistRow.DataTypes.Add(header.HeaderType);
            checklistRow.Data.Add(null);
        }
    }

    public bool AddRow(ChecklistRow row)
    {
        if (!CanHaveRow(row)) return false;
        
        Rows.Add(row);

        return true;
    }

    public bool CanHaveRow(ChecklistRow row)
    {
        if (row.DataTypes.Count != Headers.Count)
        {
            return false;
        }

        return !Headers.Where((t, i) => row.DataTypes[i] != t.HeaderType).Any();
    }
}