using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Data
{
    public sealed class ReadingOrders
    {
        private static Dictionary<int, ReadingOrder>? _items;
        public static ref Dictionary<int, ReadingOrder>? Items 
        {
            get
            {
                if (_items == null)
                {
                    _items = [];
                }
                return ref _items;
            }
        }
    }
}
