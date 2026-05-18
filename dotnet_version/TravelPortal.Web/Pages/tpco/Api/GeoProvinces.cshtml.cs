using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Utils;

namespace TravelPortal.Web.Pages.tpco.Api;

public class GeoProvincesModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public GeoProvincesModel(ISqlSugarClient db) => _db = db;

    public IActionResult OnGet()
    {
        // 顶级只返回省份 (Level 2)
        var domestic = _db.Queryable<Geo>()
            .Where(g => g.Level == 2 && g.Nature == "Domestic") 
            .OrderBy(g => g.SortOrder)
            .OrderBy(g => g.Title)
            .ToList()
            .Select(g => new { 
                g.Id, 
                g.Title, 
                g.Level,
                FirstLetter = PinyinHelper.GetFirstLetter(g.Title)
            })
            .ToList();

        // 境外顶级
        var overseas = _db.Queryable<Geo>()
            .Where(g => g.Level == 1 && g.Nature == "Overseas")
            .OrderBy(g => g.SortOrder)
            .OrderBy(g => g.Title)
            .ToList()
            .Select(g => new { 
                g.Id, 
                g.Title, 
                g.Level,
                FirstLetter = PinyinHelper.GetFirstLetter(g.Title)
            })
            .ToList();

        return new JsonResult(new { 
            Domestic = domestic, 
            Overseas = overseas 
        });
    }
}
