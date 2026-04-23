using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public SelectList GeoList { get; set; } = null!;

    public IActionResult OnGet(int id)
    {
        Spot = _db.Queryable<ScenicSpot>().InSingle(id);
        if (Spot == null) return NotFound();
        LoadData();
        return Page();
    }

    private void LoadData()
    {
        ScenicCategories = _db.Queryable<HotWord>()
            .Where(it => it.ShowInScenic)
            .Select(it => it.Name)
            .ToList();

        var geos = _db.Queryable<Geo>().Where(it => it.Level >= 2).ToList();
        GeoList = new SelectList(geos, "Id", "Title");
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
