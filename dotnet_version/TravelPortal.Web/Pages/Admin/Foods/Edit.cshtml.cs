using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.Foods;

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
    public Food Food { get; set; } = null!;

    [BindProperty]
    public IFormFile? MainImageFile { get; set; }

    public List<string> CuisineCategories { get; set; } = new();
    public List<string> SpecialtyCategories { get; set; } = new();
    public string? CurrentGeoTitle { get; set; }

    public void OnGet(int id)
    {
        Food = _db.Queryable<Food>().InSingle(id);
        LoadData();
        CurrentGeoTitle = Food.GeoId.HasValue ? _db.Queryable<Geo>().Where(g => g.Id == Food.GeoId).Select(g => g.Title).First() : null;
    }

    private void LoadData()
    {
        CuisineCategories = _db.Queryable<HotWord>()
            .Where(it => it.ShowInCuisine)
            .Select(it => it.Name)
            .ToList();

        SpecialtyCategories = _db.Queryable<HotWord>()
            .Where(it => it.ShowInSpecialty)
            .Select(it => it.Name)
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (MainImageFile != null)
        {
            Food.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "foods");
        }

        // 处理逻辑与 Create 一致
        if (Food.ProductType == "美食")
        {
            Food.SpecialtyLevel = null;
        }

        Food.UpdatedAt = DateTime.Now;

        _db.Updateable(Food).ExecuteCommand();

        return RedirectToPage("./Index");
    }

    public async Task<JsonResult> OnPostUploadMediaAsync(IFormFile file)
    {
        var url = await _uploadService.UploadFileAsync(file, "foods");
        return new JsonResult(new { location = url });
    }
}
