using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Foods;

public class DetailsModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public DetailsModel(ISqlSugarClient db) => _db = db;

    public Food? Item { get; set; }
    public List<Food> Related { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        Item = await _db.Queryable<Food>().InSingleAsync(id.Value);
        if (Item == null || Item.IsHidden) return NotFound();

        await _db.Updateable<Food>()
            .SetColumns(f => f.Views == f.Views + 1).Where(f => f.Id == id.Value).ExecuteCommandAsync();

        Related = await _db.Queryable<Food>()
            .Where(f => f.ProductType == Item.ProductType && f.Id != id.Value && !f.IsHidden)
            .OrderByDescending(f => f.CreatedAt).Take(6)
            .Select(f => new Food { Id = f.Id, Title = f.Title, CreatedAt = f.CreatedAt })
            .ToListAsync();

        return Page();
    }
}
