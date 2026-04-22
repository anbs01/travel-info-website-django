using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Transports;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public IndexModel(ISqlSugarClient db)
    {
        _db = db;
    }

    public List<Transport> TransportList { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int? GeoId { get; set; }

    public void OnGet()
    {
        TransportList = _db.Queryable<Transport>()
            .LeftJoin<Geo>((t, g) => t.GeoId == g.Id)
            .WhereIF(GeoId.HasValue, t => t.GeoId == GeoId)
            .OrderByDescending(t => t.CreatedAt)
            .Select((t, g) => new Transport
            {
                Id = t.Id,
                Title = t.Title,
                TransportType = t.TransportType,
                Geo = new Geo { Title = g.Title },
                CreatedAt = t.CreatedAt,
                IsHidden = t.IsHidden
            })
            .ToList();
    }

    public IActionResult OnPostDelete(int id)
    {
        _db.Deleteable<Transport>().In(id).ExecuteCommand();
        return RedirectToPage();
    }
}
