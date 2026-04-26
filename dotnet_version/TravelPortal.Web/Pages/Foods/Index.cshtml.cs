using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Foods;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public IndexModel(ISqlSugarClient db) => _db = db;

    public PaginatedList<Food> FoodList { get; set; } = null!;
    public List<string> CuisineCategories { get; set; } = new();
    public List<string> SpecialtyCategories { get; set; } = new();

    [BindProperty(SupportsGet = true)] public string? ProductType { get; set; }  // 美食/特产
    [BindProperty(SupportsGet = true)] public string? Category { get; set; }     // 子类别
    [BindProperty(SupportsGet = true)] public string? Keyword { get; set; }
    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;

    public void OnGet()
    {
        CuisineCategories = _db.Queryable<HotWord>()
            .Where(h => h.Module == HotWord.MOD_CUISINE && !h.IsHidden)
            .OrderBy(h => h.SortOrder).Select(h => h.Name).ToList();
        SpecialtyCategories = _db.Queryable<HotWord>()
            .Where(h => h.Module == HotWord.MOD_SPECIALTY && !h.IsHidden)
            .OrderBy(h => h.SortOrder).Select(h => h.Name).ToList();

        var query = _db.Queryable<Food>().Where(f => !f.IsHidden);
        if (!string.IsNullOrEmpty(ProductType)) query = query.Where(f => f.ProductType == ProductType);
        if (!string.IsNullOrEmpty(Category)) query = query.Where(f => f.Classification == Category);
        if (!string.IsNullOrEmpty(Keyword)) query = query.Where(f => f.Title.Contains(Keyword));

        int total = 0;
        var items = query.OrderByDescending(f => f.IsSticky).OrderByDescending(f => f.CreatedAt)
                         .ToPageList(PageIndex, 10, ref total);
        FoodList = new PaginatedList<Food>(items, total, PageIndex, 10);
    }
}
