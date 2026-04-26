using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.News;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public IndexModel(ISqlSugarClient db) => _db = db;

    public PaginatedList<TravelPortal.Web.Models.News> NewsList { get; set; } = null!;
    public List<string> Categories { get; set; } = new();

    [BindProperty(SupportsGet = true)] public string? Category { get; set; }
    [BindProperty(SupportsGet = true)] public string? Keyword { get; set; }
    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;

    public void OnGet()
    {
        Categories = _db.Queryable<HotWord>()
            .Where(h => h.Module == HotWord.MOD_NEWS && !h.IsHidden)
            .OrderBy(h => h.SortOrder).Select(h => h.Name).ToList();

        var query = _db.Queryable<TravelPortal.Web.Models.News>().Where(n => !n.IsHidden);
        if (!string.IsNullOrEmpty(Category)) query = query.Where(n => n.NewsCategory == Category);
        if (!string.IsNullOrEmpty(Keyword)) query = query.Where(n => n.Title.Contains(Keyword));

        int total = 0;
        var items = query.OrderByDescending(n => n.IsSticky).OrderByDescending(n => n.CreatedAt)
                         .ToPageList(PageIndex, 10, ref total);
        NewsList = new PaginatedList<TravelPortal.Web.Models.News>(items, total, PageIndex, 10);
    }
}
