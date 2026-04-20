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

        public List<Models.News> NewsList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        public void OnGet()
        {
            var query = _db.Queryable<Models.News>();

            if (!string.IsNullOrEmpty(Keyword))
            {
                query = query.Where(n => n.Title.Contains(Keyword));
            }

            if (!string.IsNullOrEmpty(Category))
            {
                query = query.Where(n => n.Category == Category);
            }

            NewsList = query.OrderByDescending(n => n.IsSticky)
                            .OrderByDescending(n => n.CreatedAt)
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
