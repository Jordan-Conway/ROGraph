using ROGraph.Data.DataProviders.Interfaces;
using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Data.DataProviders.MockProviders
{
    public class MockReadingOrderListProvider : IReadingOrderListProvider
    {
        public List<ReadingOrderOverview> GetReadingOrders()
        {
            List<ReadingOrderOverview> list = new List<ReadingOrderOverview>();
            list.Add(new ReadingOrderOverview("Reading Order 1", new Guid()));
            list.Add(new ReadingOrderOverview("Reading Order 2", new Guid()));

            return list;
        }

        public ReadingOrderOverview? GetReadingOrderOverview(Guid id)
        {
            return new ReadingOrderOverview("Reading Order 1", new Guid());
        }
    }
}
