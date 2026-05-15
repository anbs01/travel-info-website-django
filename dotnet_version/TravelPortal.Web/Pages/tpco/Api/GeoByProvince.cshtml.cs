using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Utils;

namespace TravelPortal.Web.Pages.tpco.Api;

public class GeoByProvinceModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public GeoByProvinceModel(ISqlSugarClient db) => _db = db;

    public IActionResult OnGet(int? parentId, string? search)
    {
        var query = _db.Queryable<Geo>();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(g => g.Title.Contains(search));
        }
        else if (parentId.HasValue)
        {
            query = query.Where(g => g.ParentId == parentId);
        }
        else
        {
            return new JsonResult(new List<object>());
        }

        var items = query
            .OrderBy(g => g.Title)
            .ToList()
            .Select(g => new { 
                g.Id, 
                g.Title, 
                g.Level, 
                g.ParentId,
                FirstLetter = PinyinHelper.GetFirstLetter(g.Title)
            })
            .ToList();

        return new JsonResult(items);
    }
}
