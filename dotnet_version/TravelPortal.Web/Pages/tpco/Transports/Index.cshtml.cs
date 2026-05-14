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
            .LeftJoin<Geo>((t, g) => t.GeoId == g.Id)
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
            .Select((t, g) => new Transport
            {
                Id = t.Id, Title = t.Title, TransportType = t.TransportType,
                Geo = new Geo { Title = g.Title }, CreatedAt = t.CreatedAt, 
                IsHidden = t.IsHidden, SortOrder = t.SortOrder
            })
            .ToPageListAsync(PageIndex, 10);

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
