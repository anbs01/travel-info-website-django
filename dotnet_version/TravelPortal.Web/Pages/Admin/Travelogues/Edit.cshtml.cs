using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.Travelogues
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
        public Travelogue Travelogue { get; set; } = default!;

        [BindProperty]
        public IFormFile? MainImageFile { get; set; }

        public SelectList PlaceList { get; set; } = default!;
        public SelectList RegionList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Travelogue = await _db.Queryable<Travelogue>().InSingleAsync(id);
            if (Travelogue == null) return NotFound();

            await LoadSelectionData();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectionData();
                return Page();
            }

            // 处理封面图上传
            if (MainImageFile != null)
            {
                Travelogue.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "travelogues");
            }

            // 更新数据
            await _db.Updateable(Travelogue)
                .IgnoreColumns(it => new { it.CreatedAt, it.Views }) // 保护核心元数据
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

        private async Task LoadSelectionData()
        {
            var places = await _db.Queryable<Place>().OrderBy(p => p.Title).ToListAsync();
            PlaceList = new SelectList(places, "Id", "Title");

            var regions = await _db.Queryable<Region>().OrderBy(r => r.Name).ToListAsync();
            RegionList = new SelectList(regions, "Id", "Name");
        }
    }
}
