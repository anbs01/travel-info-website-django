using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.ScenicSpots;

public class EditModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    private readonly IUploadService _uploadService;

    public EditModel(ISqlSugarClient db, IUploadService uploadService)
    {
        _db = db;
        _uploadService = uploadService;
    }

    [BindProperty] public ScenicSpot Spot { get; set; } = null!;
    [BindProperty] public IFormFile? MainImageFile { get; set; }

    public List<string> ScenicCategories { get; set; } = new();
    public string? CurrentGeoTitle { get; set; }

    public IActionResult OnGet(int id)
    {
        Spot = _db.Queryable<ScenicSpot>().InSingle(id);
        if (Spot == null) return NotFound();
        LoadData();
        CurrentGeoTitle = Spot.GeoId.HasValue ? _db.Queryable<Geo>().Where(g => g.Id == Spot.GeoId).Select(g => g.Title).First() : null;
        return Page();
    }

    private void LoadData()
    {
        ScenicCategories = _db.Queryable<HotWord>()
            .Where(it => it.ShowInScenic)
            .Select(it => it.Name)
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (MainImageFile != null)
            Spot.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "scenic");

        if (Spot.IsSticky && Spot.StickyAt == null)
            Spot.StickyAt = DateTime.Now;
        else if (!Spot.IsSticky)
            Spot.StickyAt = null;

        Spot.UpdatedAt = DateTime.Now;
        _db.Updateable(Spot).ExecuteCommand();
        return RedirectToPage("./Index");
    }

    public async Task<JsonResult> OnPostUploadMediaAsync(IFormFile file)
    {
        var url = await _uploadService.UploadFileAsync(file, "scenic");
        return new JsonResult(new { location = url });
    }
}
