using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.tpco.Geos;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public IndexModel(ISqlSugarClient db)
    {
        _db = db;
    }

    public PaginatedList<Geo> GeoList { get; set; } = null!;

    [BindProperty(SupportsGet = true)]
    public int? ParentId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? Level { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Nature { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageIndex { get; set; } = 1;

    public Geo? ParentGeo { get; set; }

    public void OnGet()
    {
        var query = _db.Queryable<Geo>();

        if (ParentId.HasValue && ParentId > 0)
        {
            query = query.Where(it => it.ParentId == ParentId);
            ParentGeo = _db.Queryable<Geo>().InSingle(ParentId.Value);
        }
        else if (Level.HasValue)
        {
            query = query.Where(it => it.Level == Level);
        }

        if (!string.IsNullOrEmpty(Nature))
            query = query.Where(it => it.Nature == Nature);

        int total = 0;
        var items = query.OrderBy(it => it.SortOrder)
                         .OrderByDescending(it => it.CreatedAt)
                         .Mapper(it =>
                         {
                             // 如果是国家，统计下级（省份）数量
                             if (it.Level == 1)
                             {
                                 it.JurisdictionLayers = _db.Queryable<Geo>().Where(g => g.ParentId == it.Id).Count().ToString();
                             }
                             // 获取父级名称（用于显示 所属省份/国家）
                             if (it.ParentId > 0)
                             {
                                 it.Parent = _db.Queryable<Geo>().InSingle(it.ParentId.Value);
                             }
                         })
                         .ToPageList(PageIndex, 10, ref total);

        GeoList = new PaginatedList<Geo>(items, total, PageIndex, 10);
    }

    public IActionResult OnPostDelete(int id)
    {
        var target = _db.Queryable<Geo>().InSingle(id);
        if (target == null) return RedirectToPage();

        // 检查是否有下级
        if (_db.Queryable<Geo>().Any(it => it.ParentId == id))
        {
            string childType = target.Level == 1 ? "省份" : "城镇/下级";
            string parentType = target.Level == 1 ? "国家" : "省份";
            TempData["Error"] = $"请先删除该{parentType}下的所有{childType}数据，再回来删除该{parentType}。";
            return RedirectToPage();
        }

        _db.Deleteable<Geo>().In(id).ExecuteCommand();
        return RedirectToPage();
    }
}
