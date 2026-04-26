using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.News;

public class DetailsModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public DetailsModel(ISqlSugarClient db) => _db = db;

    public TravelPortal.Web.Models.News? Item { get; set; }
    public List<TravelPortal.Web.Models.News> Related { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        Item = await _db.Queryable<TravelPortal.Web.Models.News>().InSingleAsync(id.Value);
        if (Item == null || Item.IsHidden) return NotFound();

        await _db.Updateable<TravelPortal.Web.Models.News>()
            .SetColumns(n => n.Views == n.Views + 1).Where(n => n.Id == id.Value).ExecuteCommandAsync();

        Related = await _db.Queryable<TravelPortal.Web.Models.News>()
            .Where(n => n.NewsCategory == Item.NewsCategory && n.Id != id.Value && !n.IsHidden)
            .OrderByDescending(n => n.CreatedAt).Take(6)
            .Select(n => new TravelPortal.Web.Models.News { Id = n.Id, Title = n.Title, CreatedAt = n.CreatedAt })
            .ToListAsync();

        return Page();
    }
}
