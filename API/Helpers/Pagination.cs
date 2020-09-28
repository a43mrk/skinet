using System.Collections.Generic;

namespace API.Helpers
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            this.PageIndex = pageIndex;
            this.Pagesize = pageSize;
            this.Count = count;
            this.Data = data;
        }
        public int PageIndex { get; set; }
        public int Pagesize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

    }
}