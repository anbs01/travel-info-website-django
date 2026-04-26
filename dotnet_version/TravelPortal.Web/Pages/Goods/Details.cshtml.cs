using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Goods;

public class DetailsModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public DetailsModel(ISqlSugarClient db) => _db = db;

    public CreativeProduct? Item { get; set; }
    public List<CreativeProduct> Related { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        Item = await _db.Queryable<CreativeProduct>().InSingleAsync(id.Value);
        if (Item == null || Item.IsHidden) return NotFound();

        await _db.Updateable<CreativeProduct>()
            .SetColumns(p => p.Views == p.Views + 1).Where(p => p.Id == id.Value).ExecuteCommandAsync();

        Related = await _db.Queryable<CreativeProduct>()
            .Where(p => p.Classification == Item.Classification && p.Id != id.Value && !p.IsHidden)
            .OrderByDescending(p => p.CreatedAt).Take(6)
            .Select(p => new CreativeProduct { Id = p.Id, Title = p.Title, CreatedAt = p.CreatedAt })
            .ToListAsync();

        return Page();
    }
}
