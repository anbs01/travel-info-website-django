using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.tpco.CreativeProducts;

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
                         .OrderBy(it => it.SortOrder)
                         .OrderByDescending(it => it.CreatedAt)
                         .ToPageList(PageIndex, 10, ref total);

        Products = new PaginatedList<CreativeProduct>(items, total, PageIndex, 10);
    }

    public IActionResult OnPostDelete(string ids)
    {
        if (!string.IsNullOrEmpty(ids))
        {
            var idArray = ids.Split(',').Select(int.Parse).ToArray();
            _db.Deleteable<CreativeProduct>().In(idArray).ExecuteCommand();
        }
        return RedirectToPage();
    }

    public IActionResult OnPostToggleSticky(string ids)
    {
        if (!string.IsNullOrEmpty(ids))
        {
            var idArray = ids.Split(',').Select(int.Parse).ToArray();
            var items = _db.Queryable<CreativeProduct>().In(idArray).ToList();
            foreach (var item in items)
            {
                item.IsSticky = !item.IsSticky;
                item.StickyAt = item.IsSticky ? DateTime.Now : null;
                _db.Updateable(item).UpdateColumns(it => new { it.IsSticky, it.StickyAt }).ExecuteCommand();
            }
        }
        return RedirectToPage();
    }

    public IActionResult OnPostToggleHidden(string ids)
    {
        if (!string.IsNullOrEmpty(ids))
        {
            var idArray = ids.Split(',').Select(int.Parse).ToArray();
            var items = _db.Queryable<CreativeProduct>().In(idArray).ToList();
            foreach (var item in items)
            {
                item.IsHidden = !item.IsHidden;
                _db.Updateable(item).UpdateColumns(it => new { it.IsHidden }).ExecuteCommand();
            }
        }
        return RedirectToPage();
    }
}
