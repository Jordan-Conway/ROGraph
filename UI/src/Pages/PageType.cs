using System;

namespace ROGraph.UI.Pages;

public abstract record PageType()
{
    public record ReadingOrderListPage : PageType;

    public record ReadingOrderPage(Guid ReadingOrderId) : PageType;
}