using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Places;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public IndexModel(ISqlSugarClient db) => _db = db;

    public PaginatedList<Geo> SpotList { get; set; } = null!;
    public List<string> Categories { get; set; } = new();
    public List<Geo> Provinces { get; set; } = new();

    [BindProperty(SupportsGet = true)] public string? Category { get; set; }
    [BindProperty(SupportsGet = true)] public string? Keyword { get; set; }
    [BindProperty(SupportsGet = true)] public int? GeoId { get; set; }
    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;

    public void OnGet()
    {
        Categories = _db.Queryable<HotWord>()
            .Where(h => h.Module == HotWord.MOD_PLACE && !h.IsHidden)
            .OrderBy(h => h.SortOrder).Select(h => h.Name).ToList();

        // 加载省份列表 (Level=2)
        Provinces = _db.Queryable<Geo>()
            .Where(g => g.Level == 2 && !g.IsHidden)
            .OrderBy(g => g.SortOrder)
            .ToList();

        // 仅查询城镇及以下级别的行政区 (Level >= 3)
        var query = _db.Queryable<Geo>().Where(g => g.Level >= 3 && !g.IsHidden);
        
        // 分类过滤（因为 Geo 暂无 Classification，如果选择海外则过滤海外，否则按标签匹配）
        if (!string.IsNullOrEmpty(Category)) 
        {
            query = query.Where(g => (g.Tags != null && g.Tags.Contains(Category)) || (g.GeoTag != null && g.GeoTag.Contains(Category)));
        }

        if (!string.IsNullOrEmpty(Keyword)) 
        {
            query = query.Where(g => (g.Title != null && g.Title.Contains(Keyword)) || (g.FullTitle != null && g.FullTitle.Contains(Keyword)));
        }
        
        if (GeoId.HasValue) 
        {
            // 获取当前地区及其子地区的 ID 集合
            var targetIds = _db.Queryable<Geo>()
                .Where(g => g.Id == GeoId || g.ParentId == GeoId)
                .Select(g => g.Id).ToList();
            query = query.Where(g => targetIds.Contains(g.ParentId ?? 0) || targetIds.Contains(g.Id));
        }

        int total = 0;
        var items = query.OrderByDescending(g => g.IsSticky).OrderByDescending(g => g.CreatedAt)
                         .ToPageList(PageIndex, 15, ref total); 
        SpotList = new PaginatedList<Geo>(items, total, PageIndex, 15);
    }
}
