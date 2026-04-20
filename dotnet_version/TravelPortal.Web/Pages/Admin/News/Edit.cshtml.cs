using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.News
{
    public class EditModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;
        private readonly IUploadService _uploadService;

        public EditModel(ISqlSugarClient db, IUploadService uploadService)
        {
            _db = db;
            _uploadService = uploadService;
        }

        [BindProperty]
        public Models.News News { get; set; } = default!;

        [BindProperty]
        public IFormFile? MainImageFile { get; set; }

        public List<HotWord> NewsCategories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            News = await _db.Queryable<Models.News>().InSingleAsync(id);
            if (News == null) return NotFound();

            await LoadDataAsync();
            return Page();
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

            // 执行更新
            await _db.Updateable(News)
                .IgnoreColumns(it => new { it.CreatedAt, it.Views })
                .ExecuteCommandAsync();

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
            NewsCategories = await _db.Queryable<HotWord>()
                .Where(h => h.ShowInNews && !h.IsHidden)
                .OrderBy(h => h.SortOrder)
                .ToListAsync();
        }
    }
}
