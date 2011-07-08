using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.ViewModels
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize) :
            this(source.Skip(pageIndex * pageSize).Take(pageSize), pageIndex, pageSize, x => x.Count()) { }

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, Func<IEnumerable<T>, int> totalFunc)
        {
            var totalCount = totalFunc(source);
            this.TotalCount = totalCount;
            this.TotalPages = totalCount / pageSize;

            if (totalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;

            this.AddRange(source.ToList());
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasPreviousPage { get { return (PageIndex > 0); } }
        public bool HasNextPage { get { return (PageIndex + 1 < TotalPages); } }
    }
}
