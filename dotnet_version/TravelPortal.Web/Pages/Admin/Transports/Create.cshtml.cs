using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Transports;

public class CreateModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public CreateModel(ISqlSugarClient db)
    {
        _db = db;
    }

    [BindProperty]
    public Transport Transport { get; set; } = new();

    public void OnGet(int? geoId)
    {
        Transport.GeoId = geoId ?? 0;
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _db.Insertable(Transport).ExecuteCommand();
        return RedirectToPage("Index");
    }
}
