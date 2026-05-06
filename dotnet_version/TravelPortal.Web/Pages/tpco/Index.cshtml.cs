using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TravelPortal.Web.Pages.tpco
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // 访问后台根目录时，自动跳转到城镇乡村管理模块
            return RedirectToPage("/tpco/Travelogues/Index");
        }
    }
}
