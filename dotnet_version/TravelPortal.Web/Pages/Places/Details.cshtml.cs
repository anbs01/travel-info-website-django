using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Places;

public class DetailsModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public DetailsModel(ISqlSugarClient db) => _db = db;

    public Geo? Item { get; set; }
    public List<Geo> ParentChain { get; set; } = new();
    
    // 子级内容聚合
    public List<ScenicSpot> ScenicSpots { get; set; } = new();
    public List<Food> Foods { get; set; } = new();
    public List<Travelogue> Travelogues { get; set; } = new();
    public List<Transport> Transports { get; set; } = new();
    public List<CreativeProduct> CreativeProducts { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        // 1. 获取当前地理实体
        Item = await _db.Queryable<Geo>().InSingleAsync(id.Value);
        if (Item == null || Item.IsHidden) return NotFound();

        // 增加阅读量
        await _db.Updateable<Geo>()
            .SetColumns(g => g.Views == g.Views + 1)
            .Where(g => g.Id == id.Value)
            .ExecuteCommandAsync();

        // 加载父级链（用于面包屑）
        if (Item.ParentId > 0)
        {
            var allGeos = await _db.Queryable<Geo>().ToListAsync();
            var current = Item;
            while (current != null && current.ParentId > 0)
            {
                var parent = allGeos.FirstOrDefault(g => g.Id == current.ParentId);
                if (parent == null) break;
                ParentChain.Insert(0, parent);
                current = parent;
            }
        }

        // 2. 加载关联内容 (根据 GeoId)
        ScenicSpots = await _db.Queryable<ScenicSpot>()
            .Where(s => s.GeoId == id.Value && !s.IsHidden)
            .OrderByDescending(s => s.IsSticky).OrderByDescending(s => s.CreatedAt)
            .Take(12).ToListAsync();

        Foods = await _db.Queryable<Food>()
            .Where(f => f.GeoId == id.Value && !f.IsHidden)
            .OrderByDescending(f => f.IsSticky).OrderByDescending(f => f.CreatedAt)
            .Take(12).ToListAsync();

        Travelogues = await _db.Queryable<Travelogue>()
            .Where(t => t.GeoId == id.Value && !t.IsHidden)
            .OrderByDescending(t => t.IsSticky).OrderByDescending(t => t.CreatedAt)
            .Take(12).ToListAsync();

        CreativeProducts = await _db.Queryable<CreativeProduct>()
            .Where(c => c.GeoId == id.Value && !c.IsHidden)
            .OrderByDescending(c => c.IsSticky).OrderByDescending(c => c.CreatedAt)
            .Take(12).ToListAsync();

        // 3. 加载交通信息并按特定业务顺序排序
        // 顺序：机场 > 火车站 > 汽车站 > 码头
        var transportData = await _db.Queryable<Transport>()
            .Where(t => t.GeoId == id.Value && !t.IsHidden)
            .ToListAsync();

        var typeOrder = new List<string> { "Airport", "Railway", "Bus", "Pier" };
        Transports = transportData
            .OrderBy(t => {
                int idx = typeOrder.IndexOf(t.TransportType ?? "");
                return idx == -1 ? 99 : idx;
            })
            .ThenByDescending(t => t.CreatedAt)
            .ToList();

        return Page();
    }
}
