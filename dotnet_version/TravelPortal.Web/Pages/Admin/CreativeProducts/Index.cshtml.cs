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

    public List<CreativeProduct> Products { get; set; } = new();
    public List<string> Categories { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public void OnGet()
    {
        // 加载分类列表 (从热词库中读取)
        Categories = _db.Queryable<HotWord>()
            .Where(it => it.Module == HotWord.MOD_CREATIVE && !it.IsHidden)
            .OrderBy(it => it.SortOrder)
            .Select(it => it.Name)
            .ToList();

        var query = _db.Queryable<CreativeProduct>();

        if (!string.IsNullOrEmpty(Category))
        {
            query = query.Where(it => it.Classification == Category);
        }

        if (!string.IsNullOrEmpty(Search))
        {
            query = query.Where(it => it.Title.Contains(Search));
        }

        Products = query.OrderByDescending(it => it.IsSticky)
                        .OrderByDescending(it => it.CreatedAt)
                        .ToList();
    }

    public IActionResult OnPostDelete(int id)
    {
        _db.Deleteable<CreativeProduct>().In(id).ExecuteCommand();
        return RedirectToPage();
    }
}
