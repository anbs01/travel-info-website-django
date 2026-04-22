using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Geos;

public class CreateModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public CreateModel(ISqlSugarClient db)
    {
        _db = db;
    }

    [BindProperty]
    public Geo Geo { get; set; } = new();

    public List<SelectListItem> ParentOptions { get; set; } = new();

    public void OnGet(int? parentId, int? level, string? nature)
    {
        Geo.ParentId = parentId;
        Geo.Level = level ?? 1;
        Geo.Nature = nature ?? "Domestic";

        LoadParents();
    }

    private void LoadParents()
    {
        // 只能选择比当前层级小 1 级的作为父级
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

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadParents();
            return Page();
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

        _db.Insertable(Geo).ExecuteCommand();
        return RedirectToPage("Index", new { parentId = Geo.ParentId, level = Geo.Level, nature = Geo.Nature });
    }
}
