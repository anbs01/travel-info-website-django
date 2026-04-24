using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.ScenicSpots;

public class CreateModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    private readonly IUploadService _uploadService;

    public CreateModel(ISqlSugarClient db, IUploadService uploadService)
    {
        _db = db;
        _uploadService = uploadService;
    }

    [BindProperty] public ScenicSpot Spot { get; set; } = new();
    [BindProperty] public IFormFile? MainImageFile { get; set; }

    public List<string> ScenicCategories { get; set; } = new();

    public void OnGet() => LoadData();

    private void LoadData()
    {
        ScenicCategories = _db.Queryable<HotWord>()
            .Where(it => it.Module == HotWord.MOD_SCENIC && !it.IsHidden)
            .OrderBy(it => it.SortOrder)
            .Select(it => it.Name)
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (MainImageFile != null)
            Spot.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "scenic");

        Spot.CreatedAt = DateTime.Now;
        Spot.UpdatedAt = DateTime.Now;

        _db.Insertable(Spot).ExecuteCommand();
        return RedirectToPage("./Index");
    }

    public async Task<JsonResult> OnPostUploadMediaAsync(IFormFile file)
    {
        var url = await _uploadService.UploadFileAsync(file, "scenic");
        return new JsonResult(new { location = url });
    }
}
