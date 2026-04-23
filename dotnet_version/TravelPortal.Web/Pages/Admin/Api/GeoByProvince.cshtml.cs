using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Api;

public class GeoByProvinceModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public GeoByProvinceModel(ISqlSugarClient db) => _db = db;

    /// <summary>
    /// 返回指定省 ID 下所有子孙节点，按 Level 分组
    /// GET /Admin/Api/GeoByProvince?provinceId=10
    /// </summary>
    public IActionResult OnGet(int provinceId)
    {
        // 取该省下所有后代（通过 AncestorPath 或递归 ParentId）
        // 用 AncestorPath LIKE '%/provinceId/%' 或 ParentId 递归
        // 这里用简单递归查询，层级不深性能够用
        var all = _db.Queryable<Geo>()
            .Where(g => g.Level >= 3)
            .Select(g => new { g.Id, g.Title, g.Level, g.ParentId })
            .ToList();

        // 找出属于该省的所有后代
        var provinceDescendants = GetDescendants(all
            .Select(x => new Geo { Id = x.Id, Title = x.Title, Level = x.Level, ParentId = x.ParentId })
            .ToList(), provinceId);

        // 按 Level 分组
        var grouped = provinceDescendants
            .GroupBy(g => g.Level)
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                level = g.Key,
                label = g.Key switch { 3 => "地级市", 4 => "县级市", 5 => "乡镇", 6 => "村庄", _ => $"{g.Key}级" },
                items = g.OrderBy(x => x.Title).Select(x => new { x.Id, x.Title }).ToList()
            })
            .ToList();

        return new JsonResult(grouped);
    }

    private List<Geo> GetDescendants(List<Geo> all, int parentId)
    {
        var result = new List<Geo>();
        var children = all.Where(g => g.ParentId == parentId).ToList();
        foreach (var child in children)
        {
            result.Add(child);
            result.AddRange(GetDescendants(all, child.Id));
        }
        return result;
    }
}
