using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Infrastructure.Extensions;
using NHibernate.Linq;


namespace BaseMVC.Infrastructure
{
    public class DataPage<TData>
    {
        public DataPage(IEnumerable<TData> items, int pageNumber, int totalItemsCount, int pageSize)
        {            
            Items           = items.ToList();
            PageNumber      = pageNumber;
            TotalItemsCount = totalItemsCount;
            PageSize        = pageSize;
        }

        public IEnumerable<TData> Items { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalItemsCount { get; private set; }
        public int PageSize { get; private set; }

        public static DataPage<TData> GetDataPage(IQueryable<TData> query, int pageSize, int pageNumber)
        {
            var firstResult = pageSize * (pageNumber - 1);
            var totalCount  = query.ToFutureValue(x => x.Count());
            var entities    = query.Skip(firstResult).Take(pageSize).ToFuture();
            var ble = entities.ToList(); 
            var page        = new DataPage<TData>(entities, pageNumber, totalCount.Value, pageSize);
            
            return page;
        }
    }
}
