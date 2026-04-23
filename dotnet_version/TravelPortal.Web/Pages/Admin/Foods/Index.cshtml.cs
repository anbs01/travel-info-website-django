using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Foods;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public IndexModel(ISqlSugarClient db)
    {
        _db = db;
    }

    public PaginatedList<Food> FoodList { get; set; } = null!;
    
    [BindProperty(SupportsGet = true)]
    public string? Keyword { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SpecialtyCategory { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? GeoId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageIndex { get; set; } = 1;

    public List<string> SpecialtyCategories { get; set; } = new();

    public void OnGet()
    {
        SpecialtyCategories = _db.Queryable<HotWord>()
            .Where(it => it.ShowInSpecialty)
            .Select(it => it.Name)
            .ToList();

        var query = _db.Queryable<Food>()
            .LeftJoin<Geo>((f, r) => f.GeoId == r.Id)
            .WhereIF(!string.IsNullOrEmpty(Keyword), f => f.Title.Contains(Keyword!))
            .WhereIF(!string.IsNullOrEmpty(Category), f => f.ProductType == Category)
            .WhereIF(!string.IsNullOrEmpty(SpecialtyCategory), f => f.Classification == SpecialtyCategory)
            .OrderByDescending(f => f.IsSticky)
            .OrderByDescending(f => f.CreatedAt)
            .Select((f, r) => new Food
            {
                Id = f.Id,
                Title = f.Title,
                ProductType = f.ProductType,
                Views = f.Views,
                IsSticky = f.IsSticky,
                IsHidden = f.IsHidden,
                CreatedAt = f.CreatedAt,
                Geo = new Geo { Title = r.Title }
            });

        int totalCount = 0;
        var items = query.ToPageList(PageIndex, 10, ref totalCount);

        FoodList = new PaginatedList<Food>(items, totalCount, PageIndex, 10);
    }

    public IActionResult OnPostDelete(int[] ids)
    {
        if (ids != null && ids.Length > 0)
        {
            _db.Deleteable<Food>().In(ids).ExecuteCommand();
        }
        return RedirectToPage();
    }

    public IActionResult OnPostToggleSticky(int[] ids)
    {
        if (ids != null && ids.Length > 0)
        {
            var items = _db.Queryable<Food>().In(ids).ToList();
            foreach (var item in items)
            {
                item.IsSticky = !item.IsSticky;
                item.StickyAt = item.IsSticky ? DateTime.Now : null;
                _db.Updateable(item).UpdateColumns(it => new { it.IsSticky, it.StickyAt }).ExecuteCommand();
            }
        }
        return RedirectToPage();
    }
}
