using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.News
{
    public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public IndexModel(ISqlSugarClient db)
        {
            _db = db;
        }

        public PaginatedList<Models.News> NewsList { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public List<HotWord> NewsCategories { get; set; } = new();

        public void OnGet()
        {
            var query = _db.Queryable<Models.News>()
                .WhereIF(!string.IsNullOrEmpty(Keyword), n => n.Title.Contains(Keyword!))
                .WhereIF(!string.IsNullOrEmpty(Category), n => n.NewsCategory == Category)
                .OrderByDescending(n => n.IsSticky)
                .OrderByDescending(n => n.CreatedAt);

            int totalCount = 0;
            var items = query.ToPageList(PageIndex, 10, ref totalCount);
            NewsList = new PaginatedList<Models.News>(items, totalCount, PageIndex, 10);

            NewsCategories = _db.Queryable<HotWord>()
                .Where(h => h.Module == HotWord.MOD_NEWS && !h.IsHidden)
                .OrderBy(h => h.SortOrder)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                await _db.Deleteable<Models.News>().In(ids).ExecuteCommandAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleStickyAsync(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                var list = await _db.Queryable<Models.News>().In(ids).ToListAsync();
                foreach (var item in list)
                {
                    item.IsSticky = !item.IsSticky;
                    await _db.Updateable(item).UpdateColumns(n => n.IsSticky).ExecuteCommandAsync();
                }
            }
            return RedirectToPage();
        }
    }
}
