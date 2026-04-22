using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.Foods;

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
    public Food Food { get; set; } = new();

    [BindProperty]
    public IFormFile? MainImageFile { get; set; }

    public List<string> CuisineCategories { get; set; } = new();
    public List<string> SpecialtyCategories { get; set; } = new();
    public SelectList GeoList { get; set; } = null!;

    public void OnGet()
    {
        LoadData();
        Food.Category = "美食"; // 默认选中美食
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

        var geos = _db.Queryable<Geo>().Where(it => it.Level >= 2).ToList();
        GeoList = new SelectList(geos, "Id", "Title");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (MainImageFile != null)
        {
            Food.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "foods");
        }

        // 处理根据分类清除不相关的字段
        if (Food.Category == "美食")
        {
            Food.SpecialtyCategory = null;
            Food.SpecialtyLevel = null;
        }
        else
        {
            Food.Cuisine = null;
        }

        Food.CreatedAt = DateTime.Now;
        Food.UpdatedAt = DateTime.Now;

        _db.Insertable(Food).ExecuteCommand();

        return RedirectToPage("./Index");
    }

    // TinyMCE 统一上传接口
    public async Task<JsonResult> OnPostUploadMediaAsync(IFormFile file)
    {
        var url = await _uploadService.UploadFileAsync(file, "foods");
        return new JsonResult(new { location = url });
    }
}
