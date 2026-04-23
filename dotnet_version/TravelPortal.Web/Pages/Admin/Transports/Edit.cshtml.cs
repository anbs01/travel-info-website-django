using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Transports;

public class EditModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public EditModel(ISqlSugarClient db)
    {
        _db = db;
    }

    [BindProperty]
    public Transport Transport { get; set; } = new();

    public List<SelectListItem> GeoOptions { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        Transport = _db.Queryable<Transport>().InSingle(id);
        if (Transport == null) return NotFound();
        LoadGeos();
        return Page();
    }

    private void LoadGeos()
    {
        var geos = _db.Queryable<Geo>()
            .Where(it => it.Level >= 3)
            .OrderBy(it => it.Level)
            .OrderBy(it => it.SortOrder)
            .ToList();

        GeoOptions = geos.Select(it => new SelectListItem
        {
            Value = it.Id.ToString(),
            Text = $"[{it.Level}级] {it.Title}",
            Selected = it.Id == Transport.GeoId
        }).ToList();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadGeos();
            return Page();
        }

        _db.Updateable(Transport).ExecuteCommand();
        return RedirectToPage("Index");
    }
}
