using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.ViewModels
{
    public class DataPage<TData>
    {
        public DataPage(IEnumerable<TData> items, int pageNumber, int totalItemsCount, int pageSize)
        {
            Items           = items.ToArray();
            PageNumber      = pageNumber;
            TotalItemsCount = totalItemsCount;
            PageSize        = pageSize;
        }

        public IEnumerable<TData> Items { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalItemsCount { get; private set; }
        public int PageSize { get; private set; }
    }
}
