using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Geos;

public class EditModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public EditModel(ISqlSugarClient db)
    {
        _db = db;
    }

    [BindProperty]
    public Geo Geo { get; set; } = new();

    public List<SelectListItem> ParentOptions { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        Geo = _db.Queryable<Geo>().InSingle(id);
        if (Geo == null) return NotFound();

        LoadParents();
        return Page();
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
        else
        {
            Geo.AncestorPath = null;
        }

        _db.Updateable(Geo).ExecuteCommand();
        return RedirectToPage("Index", new { parentId = Geo.ParentId, level = Geo.Level, nature = Geo.Nature });
    }
}
