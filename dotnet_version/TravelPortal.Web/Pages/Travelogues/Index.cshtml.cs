using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Travelogues
{
    public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;
        public IndexModel(ISqlSugarClient db) => _db = db;

        public PaginatedList<Travelogue> Travelogues { get; set; } = null!;
        public List<string> SubCategories { get; set; } = new();

        // 筛选参数
        [Microsoft.AspNetCore.Mvc.BindProperty(SupportsGet = true)]
        public string? Category { get; set; }   // travelogue / guide

        [Microsoft.AspNetCore.Mvc.BindProperty(SupportsGet = true)]
        public string? Tag { get; set; }         // 子类别热词

        [Microsoft.AspNetCore.Mvc.BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        [Microsoft.AspNetCore.Mvc.BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public void OnGet()
        {
            // 加载当前分类下的子类别热词
            var module = Category == "guide" ? HotWord.MOD_GUIDE : HotWord.MOD_TRAVELOGUE;
            SubCategories = _db.Queryable<HotWord>()
                .Where(h => h.Module == module && !h.IsHidden)
                .OrderBy(h => h.SortOrder)
                .Select(h => h.Name)
                .ToList();

            var query = _db.Queryable<Travelogue>().Where(t => !t.IsHidden);

            if (!string.IsNullOrEmpty(Category))
                query = query.Where(t => t.Classification == Category);

            if (!string.IsNullOrEmpty(Tag))
                query = query.Where(t => t.Tags != null && t.Tags.Contains(Tag));

            if (!string.IsNullOrEmpty(Keyword))
                query = query.Where(t => t.Title.Contains(Keyword));

            int total = 0;
            var items = query
                .OrderByDescending(t => t.IsSticky)
                .OrderByDescending(t => t.PublishDate)
                .OrderByDescending(t => t.CreatedAt)
                .ToPageList(PageIndex, 10, ref total);

            Travelogues = new PaginatedList<Travelogue>(items, total, PageIndex, 10);
        }
    }
}
