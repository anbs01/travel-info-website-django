using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.tpco.Travelogues
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
        public Travelogue Travelogue { get; set; } = new();

        // 动态加载的热词列表
        public List<HotWord> TravelogueCategories { get; set; } = new();
        public List<HotWord> GuideCategories { get; set; } = new();

        [BindProperty]
        public string[] SelectedTags { get; set; } = Array.Empty<string>();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
            Travelogue.Classification = "纪行";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            // 处理多选标签
            if (SelectedTags != null && SelectedTags.Length > 0)
            {
                Travelogue.Tags = string.Join(",", SelectedTags);
            }

            // 处理图片上传
            if (ImageFile != null)
            {
                Travelogue.MainImage = await _uploadService.UploadFileAsync(ImageFile, "travelogues");
            }

            // 自动填充作者：优先取 Nickname，其次取 Username
            if (string.IsNullOrEmpty(Travelogue.Author))
            {
                Travelogue.Author = User.FindFirst("Nickname")?.Value ?? User.Identity?.Name ?? "管理员";
            }

            // 增强 Slug 唯一性：时间戳 + 4位随机码
            var random = new Random();
            Travelogue.Slug = $"{DateTime.Now:yyyyMMddHHmm}{random.Next(1000, 9999)}";

            // 确保发布日期有值
            Travelogue.PublishDate ??= DateTime.Now;

            await _db.Insertable(Travelogue).ExecuteCommandAsync();

            TempData["SuccessMessage"] = "作品已成功发布";
            return RedirectToPage("Index");
        }

        /// <summary>
        /// 编辑器媒体上传接口 (TinyMCE 专用)
        /// </summary>
        public async Task<JsonResult> OnPostUploadMediaAsync(IFormFile file)
        {
            if (file == null) return new JsonResult(new { error = "No file uploaded" });
            
            // 存入 media 子目录
            var url = await _uploadService.UploadFileAsync(file, "media");
            return new JsonResult(new { location = url });
        }

        private async Task LoadDataAsync()
        {
            // 加载热词分类
            TravelogueCategories = await _db.Queryable<HotWord>()
                .Where(h => h.Module == HotWord.MOD_TRAVELOGUE && !h.IsHidden)
                .OrderBy(h => h.SortOrder).ToListAsync();

            GuideCategories = await _db.Queryable<HotWord>()
                .Where(h => h.Module == HotWord.MOD_GUIDE && !h.IsHidden)
                .OrderBy(h => h.SortOrder).ToListAsync();
        }
    }
}
