using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Places;

public class DetailsModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public DetailsModel(ISqlSugarClient db) => _db = db;

    public ScenicSpot? Item { get; set; }
    public List<ScenicSpot> Related { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        Item = await _db.Queryable<ScenicSpot>().InSingleAsync(id.Value);
        if (Item == null || Item.IsHidden) return NotFound();

        await _db.Updateable<ScenicSpot>()
            .SetColumns(s => s.Views == s.Views + 1).Where(s => s.Id == id.Value).ExecuteCommandAsync();

        Related = await _db.Queryable<ScenicSpot>()
            .Where(s => s.Classification == Item.Classification && s.Id != id.Value && !s.IsHidden)
            .OrderByDescending(s => s.CreatedAt).Take(6)
            .Select(s => new ScenicSpot { Id = s.Id, Title = s.Title, CreatedAt = s.CreatedAt })
            .ToListAsync();

        return Page();
    }
}
