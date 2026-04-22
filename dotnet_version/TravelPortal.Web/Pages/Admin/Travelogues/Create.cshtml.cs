using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.Travelogues
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

        public SelectList GeoList { get; set; } = default!;

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
            Travelogue.Category = "travelogue";
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

            Travelogue.Slug = DateTime.Now.ToString("yyyyMMddHHmm");
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
            // 加载地理节点 (省、市、县、乡、村)
            var geos = await _db.Queryable<Geo>()
                .Where(it => it.Level >= 2)
                .OrderBy(it => it.Level)
                .OrderBy(it => it.SortOrder)
                .ToListAsync();
            GeoList = new SelectList(geos, "Id", "Title");

            // 加载热词分类
            TravelogueCategories = await _db.Queryable<HotWord>()
                .Where(h => h.ShowInTravelogue && !h.IsHidden)
                .OrderBy(h => h.SortOrder).ToListAsync();

            GuideCategories = await _db.Queryable<HotWord>()
                .Where(h => h.ShowInGuide && !h.IsHidden)
                .OrderBy(h => h.SortOrder).ToListAsync();
        }
    }
}
