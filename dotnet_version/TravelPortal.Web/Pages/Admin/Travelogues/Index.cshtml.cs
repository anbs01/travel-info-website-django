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

        public PaginatedList<Travelogue> Travelogues { get; set; } = null!;
        public SelectList GeoList { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? GeoId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public async Task OnGetAsync()
        {
            var geos = await _db.Queryable<Geo>().Where(it => it.Level >= 2).OrderBy(it => it.Level).OrderBy(it => it.SortOrder).ToListAsync();
            GeoList = new SelectList(geos, "Id", "Title");

            var query = _db.Queryable<Travelogue>();

            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "Sticky") query = query.Where(t => t.IsSticky);
                if (Status == "Hidden") query = query.Where(t => t.IsHidden);
                if (Status == "Normal") query = query.Where(t => !t.IsSticky && !t.IsHidden);
            }
            if (GeoId.HasValue) query = query.Where(t => t.GeoId == GeoId);
            if (!string.IsNullOrEmpty(Keyword)) query = query.Where(t => t.Title.Contains(Keyword));

            RefAsync<int> total = 0;
            var items = await query
                .OrderByDescending(t => t.IsSticky)
                .OrderByDescending(t => t.StickyAt)
                .OrderByDescending(t => t.CreatedAt)
                .ToPageListAsync(PageIndex, 10, total);

            Travelogues = new PaginatedList<Travelogue>(items, total, PageIndex, 10);
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
