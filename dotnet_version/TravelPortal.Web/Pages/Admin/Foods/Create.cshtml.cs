using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.Foods
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
        public Models.Food Food { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList PlaceList { get; set; } = null!;

        public void OnGet()
        {
            var places = _db.Queryable<Place>().ToList();
            PlaceList = new SelectList(places, "Id", "Title");
            Food.PublishDate = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var places = _db.Queryable<Place>().ToList();
                PlaceList = new SelectList(places, "Id", "Title");
                return Page();
            }

            if (ImageFile != null)
            {
                Food.MainImage = await _uploadService.UploadFileAsync(ImageFile, "foods");
            }

            Food.CreatedAt = DateTime.Now;
            Food.UpdatedAt = DateTime.Now;

            await _db.Insertable(Food).ExecuteCommandAsync();

            return RedirectToPage("Index");
        }
    }
}
