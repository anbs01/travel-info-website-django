using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Settings
{
    public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public IndexModel(ISqlSugarClient db)
        {
            _db = db;
        }

        [BindProperty]
        public SiteInfo SiteInfo { get; set; } = new();

        public async Task OnGetAsync()
        {
            SiteInfo = await _db.Queryable<SiteInfo>().InSingleAsync(1);
            if (SiteInfo == null)
            {
                SiteInfo = new SiteInfo { Id = 1 };
                await _db.Insertable(SiteInfo).ExecuteCommandAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _db.Updateable(SiteInfo).ExecuteCommandAsync();
            
            TempData["SuccessMessage"] = "基本信息已更新";
            return RedirectToPage();
        }
    }
}
