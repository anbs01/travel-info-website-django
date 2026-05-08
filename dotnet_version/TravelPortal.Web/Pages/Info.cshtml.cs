using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages
{
    public class InfoModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;
        public InfoModel(ISqlSugarClient db) => _db = db;

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; } = "about";

        public string PageTitle { get; set; } = string.Empty;
        public string PageContent { get; set; } = string.Empty;
        public string CurrentType => Type?.ToLower() ?? "about";

        public async Task OnGetAsync()
        {
            var info = await _db.Queryable<SiteInfo>().InSingleAsync(1);
            if (info == null) return;

            switch (CurrentType)
            {
                case "about":
                    PageTitle = "关于我们";
                    PageContent = info.About ?? "暂无内容";
                    break;
                case "service":
                    PageTitle = "服务协议";
                    PageContent = info.ServiceAgreement ?? "暂无内容";
                    break;
                case "cooperate":
                    PageTitle = "合作事宜";
                    PageContent = info.Cooperation ?? "暂无内容";
                    break;
                case "feedback":
                    PageTitle = "意见建议";
                    PageContent = "如果您有任何意见或建议，欢迎联系我们。";
                    break;
                case "contact":
                    PageTitle = "联系我们";
                    PageContent = info.ContactUs ?? "暂无内容";
                    break;
                default:
                    PageTitle = "关于我们";
                    PageContent = info.About ?? "暂无内容";
                    break;
            }
        }
    }
}
