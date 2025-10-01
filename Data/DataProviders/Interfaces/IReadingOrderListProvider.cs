using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Data.DataProviders.Interfaces
{
    public interface IReadingOrderListProvider
    {
        public abstract List<ReadingOrderOverview> GetReadingOrders();

        public abstract ReadingOrderOverview? GetReadingOrderOverview(Guid id);
    }
}
