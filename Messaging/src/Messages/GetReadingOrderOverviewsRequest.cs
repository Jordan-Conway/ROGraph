using System.Collections.Generic;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.Messaging.Messages;

public class GetReadingOrderOverviewsRequest : AsyncRequestMessage<IList<ReadingOrderOverview>>
{
    
}