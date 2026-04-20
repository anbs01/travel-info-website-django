using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Travelogues
{
    public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public IndexModel(ISqlSugarClient db)
        {
            _db = db;
        }

        public List<Travelogue> Travelogues { get; set; } = new();
        public SelectList PlaceList { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PlaceId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        public async Task OnGetAsync()
        {
            // 1. 加载筛选数据
            var places = await _db.Queryable<Place>().OrderBy(p => p.Title).ToListAsync();
            PlaceList = new SelectList(places, "Id", "Title");

            // 2. 构建查询
            var query = _db.Queryable<Travelogue>();

            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "Sticky") query = query.Where(t => t.IsSticky);
                if (Status == "Hidden") query = query.Where(t => t.IsHidden);
                if (Status == "Normal") query = query.Where(t => !t.IsSticky && !t.IsHidden);
            }

            if (PlaceId.HasValue)
            {
                query = query.Where(t => t.PlaceId == PlaceId);
            }

            if (!string.IsNullOrEmpty(Keyword))
            {
                query = query.Where(t => t.Title.Contains(Keyword));
            }

            // 3. 执行排序：置顶在前，其次按创建时间倒序
            Travelogues = await query
                .OrderByDescending(t => t.IsSticky)
                .OrderByDescending(t => t.StickyAt)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                await _db.Deleteable<Travelogue>().In(ids).ExecuteCommandAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleStickyAsync(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                var items = await _db.Queryable<Travelogue>().In(ids).ToListAsync();
                foreach (var item in items)
                {
                    item.IsSticky = !item.IsSticky;
                    item.StickyAt = item.IsSticky ? DateTime.Now : null;
                }
                await _db.Updateable(items).ExecuteCommandAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleHiddenAsync(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                var items = await _db.Queryable<Travelogue>().In(ids).ToListAsync();
                foreach (var item in items)
                {
                    item.IsHidden = !item.IsHidden;
                }
                await _db.Updateable(items).ExecuteCommandAsync();
            }
            return RedirectToPage();
        }
    }
}
