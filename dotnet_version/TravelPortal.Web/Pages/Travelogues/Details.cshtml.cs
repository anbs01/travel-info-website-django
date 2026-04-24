using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Travelogues
{
    public class DetailsModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;
        public DetailsModel(ISqlSugarClient db) => _db = db;

        public Travelogue? Travelogue { get; set; }
        public List<Travelogue> Related { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Travelogue = await _db.Queryable<Travelogue>().InSingleAsync(id.Value);
            if (Travelogue == null || Travelogue.IsHidden) return NotFound();

            // 阅读量 +1
            await _db.Updateable<Travelogue>()
                .SetColumns(t => t.Views == t.Views + 1)
                .Where(t => t.Id == id.Value)
                .ExecuteCommandAsync();

            // 相关推荐：同 Classification，排除自身，最多6条
            Related = await _db.Queryable<Travelogue>()
                .Where(t => t.Classification == Travelogue.Classification
                         && t.Id != id.Value
                         && !t.IsHidden)
                .OrderByDescending(t => t.CreatedAt)
                .Take(6)
                .Select(t => new Travelogue { Id = t.Id, Title = t.Title, CreatedAt = t.CreatedAt })
                .ToListAsync();

            return Page();
        }
    }
}
