namespace TravelPortal.Web.Models
{
    /// <summary>
    /// 通用分页元数据模型
    /// </summary>
    public class PaginationMetadata
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginationMetadata(int totalCount, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }

    /// <summary>
    /// 带分页的数据列表包装类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public PaginationMetadata Metadata { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            Metadata = new PaginationMetadata(count, pageIndex, pageSize);
        }
    }
}
