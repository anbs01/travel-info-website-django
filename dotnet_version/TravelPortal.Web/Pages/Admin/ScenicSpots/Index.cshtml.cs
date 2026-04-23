using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.ScenicSpots;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public IndexModel(ISqlSugarClient db) => _db = db;

    public PaginatedList<ScenicSpot> ScenicSpots { get; set; } = null!;

    [BindProperty(SupportsGet = true)] public string? Keyword { get; set; }
    [BindProperty(SupportsGet = true)] public string? FameLevel { get; set; }
    [BindProperty(SupportsGet = true)] public string? ScenicGrade { get; set; }
    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;

    public void OnGet()
    {
        var query = _db.Queryable<ScenicSpot>()
            .LeftJoin<Geo>((s, g) => s.GeoId == g.Id)
            .WhereIF(!string.IsNullOrEmpty(Keyword), s => s.Title.Contains(Keyword!))
            .WhereIF(!string.IsNullOrEmpty(FameLevel), s => s.FameLevel == FameLevel)
            .WhereIF(!string.IsNullOrEmpty(ScenicGrade), s => s.ScenicGrade == ScenicGrade)
            .OrderByDescending(s => s.IsSticky)
            .OrderByDescending(s => s.CreatedAt)
            .Select((s, g) => new ScenicSpot
            {
                Id = s.Id, Title = s.Title, FameLevel = s.FameLevel, ScenicGrade = s.ScenicGrade,
                Views = s.Views, IsSticky = s.IsSticky, IsHidden = s.IsHidden, CreatedAt = s.CreatedAt,
                Geo = new Geo { Title = g.Title }
            });

        int total = 0;
        var items = query.ToPageList(PageIndex, 15, ref total);
        ScenicSpots = new PaginatedList<ScenicSpot>(items, total, PageIndex, 15);
    }

    public IActionResult OnPostDelete(int[] ids)
    {
        if (ids?.Length > 0)
            _db.Deleteable<ScenicSpot>().In(ids).ExecuteCommand();
        return RedirectToPage();
    }

    public IActionResult OnPostToggleSticky(int[] ids)
    {
        if (ids?.Length > 0)
        {
            var items = _db.Queryable<ScenicSpot>().In(ids).ToList();
            foreach (var item in items)
            {
                item.IsSticky = !item.IsSticky;
                item.StickyAt = item.IsSticky ? DateTime.Now : null;
                _db.Updateable(item).UpdateColumns(it => new { it.IsSticky, it.StickyAt }).ExecuteCommand();
            }
        }
        return RedirectToPage();
    }

    public IActionResult OnPostToggleHidden(int[] ids)
    {
        if (ids?.Length > 0)
        {
            var items = _db.Queryable<ScenicSpot>().In(ids).ToList();
            foreach (var item in items)
            {
                item.IsHidden = !item.IsHidden;
                _db.Updateable(item).UpdateColumns(it => new { it.IsHidden }).ExecuteCommand();
            }
        }
        return RedirectToPage();
    }
}
