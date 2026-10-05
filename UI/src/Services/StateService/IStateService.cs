using System;

namespace ROGraph.UI.Services.StateService;

public interface IStateService
{
    public Guid SelectedReadingOrderId { get; set; }
}