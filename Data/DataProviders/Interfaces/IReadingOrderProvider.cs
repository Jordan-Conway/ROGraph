using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Data.DataProviders.Interfaces
{
    public interface IReadingOrderProvider
    {
        public abstract bool CreateReadingOrder(string name, string? description);

        public abstract ReadingOrder? GetReadingOrder(ReadingOrderOverview overview);
    }
}
