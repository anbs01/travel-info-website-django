using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.Geos;

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
    public Geo Geo { get; set; } = new();

    [BindProperty]
    public IFormFile? MainImageFile { get; set; }

    public string PageTitle { get; set; } = "编辑节点";
    public List<SelectListItem> ParentOptions { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        Geo = _db.Queryable<Geo>().InSingle(id);
        if (Geo == null) return NotFound();

        UpdatePageTitle();
        LoadParents();
        return Page();
    }

    private void UpdatePageTitle()
    {
        if (Geo.Level == 1) PageTitle = "编辑国家";
        else if (Geo.Level == 2) PageTitle = "编辑省份";
        else if (Geo.Level >= 3) PageTitle = "编辑城镇";
        if (Geo.Nature == "Overseas") PageTitle = "编辑海外城镇";
    }

    private void LoadParents()
    {
        if (Geo.Level > 1)
        {
            var parents = _db.Queryable<Geo>()
                .Where(it => it.Level == Geo.Level - 1)
                .OrderBy(it => it.SortOrder)
                .ToList();

            ParentOptions = parents.Select(it => new SelectListItem
            {
                Value = it.Id.ToString(),
                Text = $"[{it.Level}级] {it.Title}",
                Selected = it.Id == Geo.ParentId
            }).ToList();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            UpdatePageTitle();
            LoadParents();
            return Page();
        }

        if (MainImageFile != null)
        {
            Geo.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "geos");
        }

        // 处理 AncestorPath
        if (Geo.ParentId.HasValue && Geo.ParentId > 0)
        {
            var parent = _db.Queryable<Geo>().InSingle(Geo.ParentId.Value);
            if (parent != null)
            {
                Geo.AncestorPath = string.IsNullOrEmpty(parent.AncestorPath) 
                    ? parent.Id.ToString() 
                    : $"{parent.AncestorPath}/{parent.Id}";
            }
        }
        else
        {
            Geo.AncestorPath = null;
        }

        Geo.UpdatedAt = DateTime.Now;

        _db.Updateable(Geo).ExecuteCommand();
        return RedirectToPage("Index", new { parentId = Geo.ParentId, level = Geo.Level, nature = Geo.Nature });
    }

    public async Task<JsonResult> OnPostUploadMedia(IFormFile file)
    {
        if (file == null) return new JsonResult(new { error = "未选择文件" });
        var url = await _uploadService.UploadFileAsync(file, "editor/geos");
        return new JsonResult(new { location = url });
    }
}
