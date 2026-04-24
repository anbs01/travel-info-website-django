using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.News
{
    public class CreateModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;
        private readonly IUploadService _uploadService;

        public CreateModel(ISqlSugarClient db, IUploadService uploadService)
        {
            _db = db;
            _uploadService = uploadService;
        }

        [BindProperty]
        public Models.News News { get; set; } = new();

        [BindProperty]
        public IFormFile? MainImageFile { get; set; }

        public List<HotWord> NewsCategories { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            // 处理封面图上传
            if (MainImageFile != null)
            {
                News.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "news");
            }

            // 执行保存
            await _db.Insertable(News).ExecuteCommandAsync();

            return RedirectToPage("./Index");
        }

        // TinyMCE 媒体上传处理器
        public async Task<IActionResult> OnPostUploadMediaAsync(IFormFile file)
        {
            if (file == null) return new JsonResult(new { error = "No file uploaded" });
            var url = await _uploadService.UploadFileAsync(file, "media");
            return new JsonResult(new { location = url });
        }

        private async Task LoadDataAsync()
        {
            // 从综合管理（HotWord）中加载资讯类别
            NewsCategories = await _db.Queryable<HotWord>()
                .Where(h => h.Module == HotWord.MOD_NEWS && !h.IsHidden)
                .OrderBy(h => h.SortOrder)
                .ToListAsync();
        }
    }
}
