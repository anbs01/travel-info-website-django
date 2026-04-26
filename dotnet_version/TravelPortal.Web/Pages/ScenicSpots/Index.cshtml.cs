using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.ScenicSpots;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public IndexModel(ISqlSugarClient db) => _db = db;

    public PaginatedList<ScenicSpot> SpotList { get; set; } = null!;
    public List<string> Categories { get; set; } = new();

    [BindProperty(SupportsGet = true)] public string? FameLevel { get; set; }
    [BindProperty(SupportsGet = true)] public string? Category { get; set; }
    [BindProperty(SupportsGet = true)] public string? Keyword { get; set; }
    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;

    public void OnGet()
    {
        Categories = _db.Queryable<HotWord>()
            .Where(h => h.Module == HotWord.MOD_SCENIC && !h.IsHidden)
            .OrderBy(h => h.SortOrder).Select(h => h.Name).ToList();

        var query = _db.Queryable<ScenicSpot>().Where(s => !s.IsHidden);
        if (!string.IsNullOrEmpty(FameLevel)) query = query.Where(s => s.FameLevel == FameLevel);
        if (!string.IsNullOrEmpty(Category)) query = query.Where(s => s.Classification == Category);
        if (!string.IsNullOrEmpty(Keyword)) query = query.Where(s => s.Title.Contains(Keyword));

        int total = 0;
        var items = query.OrderByDescending(s => s.IsSticky).OrderByDescending(s => s.CreatedAt)
                         .ToPageList(PageIndex, 10, ref total);
        SpotList = new PaginatedList<ScenicSpot>(items, total, PageIndex, 10);
    }
}
