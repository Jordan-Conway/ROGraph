using System;

namespace ROGraph.UI.Services.StateService;

public class StateService : IStateService
{
    public Guid SelectedReadingOrderId { get; set; }
}