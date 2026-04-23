using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

    public string? CurrentGeoTitle { get; set; }

    public IActionResult OnGet(int id)
    {
        Transport = _db.Queryable<Transport>().InSingle(id);
        if (Transport == null) return NotFound();
        CurrentGeoTitle = Transport.GeoId > 0 ? _db.Queryable<Geo>().Where(g => g.Id == Transport.GeoId).Select(g => g.Title).First() : null;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _db.Updateable(Transport).ExecuteCommand();
        return RedirectToPage("Index");
    }
}
