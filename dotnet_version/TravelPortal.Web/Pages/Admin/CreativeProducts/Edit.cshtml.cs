using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.CreativeProducts;

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
    public CreativeProduct Product { get; set; } = new();

    [BindProperty]
    public IFormFile? MainImageFile { get; set; }

    public List<string> Categories { get; set; } = new();
    public string? CurrentGeoTitle { get; set; }

    public void OnGet(int id)
    {
        Product = _db.Queryable<CreativeProduct>().InSingle(id);
        LoadData();
        CurrentGeoTitle = Product.GeoId.HasValue ? _db.Queryable<Geo>().Where(g => g.Id == Product.GeoId).Select(g => g.Title).First() : null;
    }

    private void LoadData()
    {
        Categories = _db.Queryable<HotWord>()
            .Where(it => it.ShowInCreative)
            .Select(it => it.Name)
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (MainImageFile != null)
        {
            Product.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "creatives");
        }

        if (Product.IsSticky && Product.StickyAt == null)
        {
            Product.StickyAt = DateTime.Now;
        }
        else if (!Product.IsSticky)
        {
            Product.StickyAt = null;
        }

        Product.UpdatedAt = DateTime.Now;

        _db.Updateable(Product).ExecuteCommand();

        return RedirectToPage("./Index");
    }

    // TinyMCE 统一上传接口
    public async Task<JsonResult> OnPostUploadMediaAsync(IFormFile file)
    {
        var url = await _uploadService.UploadFileAsync(file, "creatives");
        return new JsonResult(new { location = url });
    }
}
