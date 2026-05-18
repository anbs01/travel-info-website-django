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
            // 搜索模式：使用 LeftJoin 联表获取父级名称，确保无父级的顶级项或断层项也能被搜索到
            var searchResults = _db.Queryable<Geo>()
                .LeftJoin<Geo>((g, p) => g.ParentId == p.Id)
                .Where((g, p) => g.Title.Contains(search))
                .Select((g, p) => new { 
                    g.Id, 
                    g.Title, 
                    g.Level, 
                    g.ParentId,
                    ParentTitle = p.Title 
                })
                .ToList()
                .Select(g => new { 
                    g.Id, 
                    g.Title, 
                    g.Level, 
                    g.ParentId,
                    g.ParentTitle,
                    FirstLetter = PinyinHelper.GetFirstLetter(g.Title)
                })
                .ToList();

            return new JsonResult(searchResults);
        }

        var items = query.Where(g => g.ParentId == parentId)
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
