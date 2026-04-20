using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Travelogues
{
    public class DetailsModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public DetailsModel(ISqlSugarClient db)
        {
            _db = db;
        }

        public Travelogue? Travelogue { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Travelogue = await _db.Queryable<Travelogue>()
                                  .InSingleAsync(id.Value);

            if (Travelogue == null || Travelogue.IsHidden)
            {
                return NotFound();
            }

            // 更新阅读量
            await _db.Updateable<Travelogue>()
                     .SetColumns(t => t.Views == t.Views + 1)
                     .Where(t => t.Id == id.Value)
                     .ExecuteCommandAsync();

            return Page();
        }
    }
}
