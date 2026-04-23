using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Api;

public class GeoProvincesModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public GeoProvincesModel(ISqlSugarClient db) => _db = db;

    /// <summary>
    /// 返回国内省列表 + 境外顶级城市列表
    /// GET /Admin/Api/GeoProvinces
    /// </summary>
    public IActionResult OnGet()
    {
        var domestic = _db.Queryable<Geo>()
            .Where(g => g.Level == 2 && g.Nature == "Domestic")
            .OrderBy(g => g.SortOrder).OrderBy(g => g.Title)
            .Select(g => new { g.Id, g.Title })
            .ToList();

        var overseas = _db.Queryable<Geo>()
            .Where(g => g.Level == 3 && g.Nature == "Overseas")
            .OrderBy(g => g.SortOrder).OrderBy(g => g.Title)
            .Select(g => new { g.Id, g.Title })
            .ToList();

        return new JsonResult(new { domestic, overseas });
    }
}
