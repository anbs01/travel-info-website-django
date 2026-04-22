using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.CreativeProducts;

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
    public CreativeProduct Product { get; set; } = new();

    [BindProperty]
    public IFormFile? MainImageFile { get; set; }

    public List<string> Categories { get; set; } = new();
    public SelectList GeoList { get; set; } = null!;

    public void OnGet()
    {
        LoadData();
    }

    private void LoadData()
    {
        Categories = _db.Queryable<HotWord>()
            .Where(it => it.ShowInCreative)
            .Select(it => it.Name)
            .ToList();

        var geos = _db.Queryable<Geo>().Where(it => it.Level >= 2).ToList();
        GeoList = new SelectList(geos, "Id", "Title");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (MainImageFile != null)
        {
            Product.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "creatives");
        }

        if (Product.IsSticky)
        {
            Product.StickyAt = DateTime.Now;
        }

        Product.CreatedAt = DateTime.Now;
        Product.UpdatedAt = DateTime.Now;

        _db.Insertable(Product).ExecuteCommand();

        return RedirectToPage("./Index");
    }

    // TinyMCE 统一上传接口
    public async Task<JsonResult> OnPostUploadMediaAsync(IFormFile file)
    {
        var url = await _uploadService.UploadFileAsync(file, "creatives");
        return new JsonResult(new { location = url });
    }
}
