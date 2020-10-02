using System.Collections.Generic;

// 65-1 Generic Pagination Model for Paginated Responses.
namespace API.Helpers
{
    public class Pagination<T> where T : class
    {
        // 65-8 don't forget the constructor that accepts all required parameters.
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Count = count;
            this.Data = data;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

    }
}