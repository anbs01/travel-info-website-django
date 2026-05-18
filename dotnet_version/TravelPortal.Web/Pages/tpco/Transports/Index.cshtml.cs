using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.tpco.Transports;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public IndexModel(ISqlSugarClient db) => _db = db;

    public PaginatedList<Transport> TransportList { get; set; } = null!;
    public SelectList GeoList { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public int? GeoId { get; set; }
    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;

    public async Task OnGetAsync()
    {
        // 加载城镇列表用于筛选
        var geos = await _db.Queryable<Geo>()
            .Where(it => it.Level >= 2)
            .OrderBy(it => it.Level).OrderBy(it => it.SortOrder).ToListAsync();
        GeoList = new SelectList(geos, "Id", "Title");

        var query = _db.Queryable<Transport>()
            .WhereIF(GeoId.HasValue, t => t.GeoId == GeoId);

        // 客户特殊排序要求：机场 > 火车站 > 汽车站 > 水上交通 > 轨道交通 > 地面公交
        // 我们利用 OrderBy 与自定义数字映射来实现
        var items = await query
            .OrderBy(t => t.SortOrder) // 依然保留手动排序作为最高优先
            .OrderBy(@"CASE TransportType 
                        WHEN 'Airport' THEN 0 
                        WHEN 'Railway' THEN 1 
                        WHEN 'Bus' THEN 2 
                        WHEN 'Pier' THEN 3 
                        WHEN 'Subway' THEN 4 
                        WHEN 'PublicBus' THEN 5 
                        ELSE 99 END ASC")
            .OrderByDescending(t => t.CreatedAt)
            .ToPageListAsync(PageIndex, 10);

        // 1. 批量拉取关联的城镇 Geo 记录
        var geoIds = items.Where(it => it.GeoId > 0).Select(it => it.GeoId!.Value).Distinct().ToList();
        if (geoIds.Any())
        {
            var geosList = await _db.Queryable<Geo>().Where(g => geoIds.Contains(g.Id)).ToListAsync();
            var geosMap = geosList.ToDictionary(g => g.Id);

            // 2. 批量拉取这些城镇关联的父级（省份） Geo 记录
            var parentIds = geosList.Where(g => g.ParentId > 0).Select(g => g.ParentId!.Value).Distinct().ToList();
            var parentsMap = new Dictionary<int, string>();
            if (parentIds.Any())
            {
                var parentsList = await _db.Queryable<Geo>().Where(p => parentIds.Contains(p.Id)).Select(p => new { p.Id, p.Title }).ToListAsync();
                parentsMap = parentsList.ToDictionary(p => p.Id, p => p.Title);
            }

            // 3. 将装配好的 Geo 对象以及父级 Title 组合赋值给 items
            foreach (var item in items)
            {
                if (item.GeoId.HasValue && geosMap.TryGetValue(item.GeoId.Value, out var geo))
                {
                    item.Geo = geo;
                    if (geo.ParentId > 0 && parentsMap.TryGetValue(geo.ParentId.Value, out var parentTitle))
                    {
                        item.Geo.GeoTag = parentTitle;
                    }
                }
            }
        }

        int totalCount = await query.CountAsync();
        TransportList = new PaginatedList<Transport>(items, totalCount, PageIndex, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int[] ids)
    {
        if (ids?.Length > 0)
            await _db.Deleteable<Transport>().In(ids).ExecuteCommandAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleHiddenAsync(int[] ids)
    {
        if (ids?.Length > 0)
        {
            var items = await _db.Queryable<Transport>().In(ids).ToListAsync();
            foreach (var item in items)
            {
                item.IsHidden = !item.IsHidden;
            }
            await _db.Updateable(items).UpdateColumns(it => new { it.IsHidden }).ExecuteCommandAsync();
        }
        return RedirectToPage();
    }
}
