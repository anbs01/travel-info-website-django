using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Places;

public class CreateModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public CreateModel(ISqlSugarClient db)
    {
        _db = db;
    }

    [BindProperty]
    public Place Input { get; set; } = new();

    public List<Region> RegionOptions { get; set; } = new();

    public void OnGet()
    {
        // 加载地区下拉选项
        RegionOptions = _db.Queryable<Region>().OrderBy(r => r.SortOrder).ToList();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            OnGet();
            return Page();
        }

        // 逻辑对齐文档：如果 FullTitle 为空，自动回退到 Title
        if (string.IsNullOrWhiteSpace(Input.FullTitle))
        {
            Input.FullTitle = Input.Title;
        }

        _db.Insertable(Input).ExecuteCommand();

        return RedirectToPage("./Index");
    }
}
