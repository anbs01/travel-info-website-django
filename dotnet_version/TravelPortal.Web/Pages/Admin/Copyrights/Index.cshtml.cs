using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Copyrights
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
            // 获取单例配置（ID=1）
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

            // 更新数据
            await _db.Updateable(SiteInfo).ExecuteCommandAsync();
            
            TempData["SuccessMessage"] = "版权引用配置已成功保存";
            return RedirectToPage();
        }
    }
}
