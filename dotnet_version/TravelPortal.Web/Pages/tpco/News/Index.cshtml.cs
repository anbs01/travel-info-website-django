using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.tpco.News
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
                .OrderBy(n => n.SortOrder)
                .OrderByDescending(n => n.CreatedAt);

            int totalCount = 0;
            var items = query.ToPageList(PageIndex, 10, ref totalCount);
            NewsList = new PaginatedList<Models.News>(items, totalCount, PageIndex, 10);

            NewsCategories = _db.Queryable<HotWord>()
                .Where(h => h.Module == HotWord.MOD_NEWS && !h.IsHidden)
                .OrderBy(h => h.SortOrder)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idArray = ids.Split(',').Select(int.Parse).ToArray();
                await _db.Deleteable<Models.News>().In(idArray).ExecuteCommandAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleStickyAsync(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idArray = ids.Split(',').Select(int.Parse).ToArray();
                var list = await _db.Queryable<Models.News>().In(idArray).ToListAsync();
                foreach (var item in list)
                {
                    item.IsSticky = !item.IsSticky;
                    await _db.Updateable(item).UpdateColumns(n => n.IsSticky).ExecuteCommandAsync();
                }
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleHiddenAsync(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idArray = ids.Split(',').Select(int.Parse).ToArray();
                var list = await _db.Queryable<Models.News>().In(idArray).ToListAsync();
                foreach (var item in list)
                {
                    item.IsHidden = !item.IsHidden;
                    await _db.Updateable(item).UpdateColumns(n => n.IsHidden).ExecuteCommandAsync();
                }
            }
            return RedirectToPage();
        }
    }
}
