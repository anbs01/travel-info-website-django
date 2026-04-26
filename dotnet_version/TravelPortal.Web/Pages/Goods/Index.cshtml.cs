using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Goods;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public IndexModel(ISqlSugarClient db) => _db = db;

    public PaginatedList<CreativeProduct> GoodsList { get; set; } = null!;
    public List<string> Categories { get; set; } = new();

    [BindProperty(SupportsGet = true)] public string? Category { get; set; }
    [BindProperty(SupportsGet = true)] public string? Keyword { get; set; }
    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;

    public void OnGet()
    {
        Categories = _db.Queryable<HotWord>()
            .Where(h => h.Module == HotWord.MOD_CREATIVE && !h.IsHidden)
            .OrderBy(h => h.SortOrder).Select(h => h.Name).ToList();

        var query = _db.Queryable<CreativeProduct>().Where(p => !p.IsHidden);
        if (!string.IsNullOrEmpty(Category)) query = query.Where(p => p.Classification == Category);
        if (!string.IsNullOrEmpty(Keyword)) query = query.Where(p => p.Title.Contains(Keyword));

        int total = 0;
        var items = query.OrderByDescending(p => p.IsSticky).OrderByDescending(p => p.CreatedAt)
                         .ToPageList(PageIndex, 10, ref total);
        GoodsList = new PaginatedList<CreativeProduct>(items, total, PageIndex, 10);
    }
}
