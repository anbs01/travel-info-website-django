using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.CreativeProducts;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public IndexModel(ISqlSugarClient db)
    {
        _db = db;
    }

    public PaginatedList<CreativeProduct> Products { get; set; } = null!;
    public List<string> Categories { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageIndex { get; set; } = 1;

    public void OnGet()
    {
        Categories = _db.Queryable<HotWord>()
            .Where(it => it.Module == HotWord.MOD_CREATIVE && !it.IsHidden)
            .OrderBy(it => it.SortOrder)
            .Select(it => it.Name)
            .ToList();

        var query = _db.Queryable<CreativeProduct>();
        if (!string.IsNullOrEmpty(Category)) query = query.Where(it => it.Classification == Category);
        if (!string.IsNullOrEmpty(Search)) query = query.Where(it => it.Title.Contains(Search));

        int total = 0;
        var items = query.OrderByDescending(it => it.IsSticky)
                         .OrderByDescending(it => it.CreatedAt)
                         .ToPageList(PageIndex, 10, ref total);

        Products = new PaginatedList<CreativeProduct>(items, total, PageIndex, 10);
    }

    public IActionResult OnPostDelete(int[] ids)
    {
        if (ids?.Length > 0)
            _db.Deleteable<CreativeProduct>().In(ids).ExecuteCommand();
        return RedirectToPage();
    }

    public IActionResult OnPostToggleSticky(int[] ids)
    {
        if (ids?.Length > 0)
        {
            var items = _db.Queryable<CreativeProduct>().In(ids).ToList();
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
