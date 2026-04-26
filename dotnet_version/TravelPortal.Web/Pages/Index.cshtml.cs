using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public IndexModel(ISqlSugarClient db) => _db = db;

    // 搜索热词
    public List<string> HomeKeywords { get; set; } = new();

    // 左侧轮播图（有图片的推荐，最近3条有效的）
    public List<Recommendation> CarouselItems { get; set; } = new();

    // 右侧列表（文字推荐 + 攻略/资讯补全，共8条）
    public List<HomeListItem> ListItems { get; set; } = new();

    public void OnGet()
    {
        var now = DateTime.Now;

        // 1. 搜索热词
        HomeKeywords = _db.Queryable<HotWord>()
            .Where(h => h.Module == HotWord.MOD_HOME && !h.IsHidden)
            .OrderBy(h => h.SortOrder)
            .Select(h => h.Name)
            .ToList();

        // 2. 左侧轮播：有图片、未过期、取最近3条
        CarouselItems = _db.Queryable<Recommendation>()
            .Where(r => r.ImageUrl != null && r.ImageUrl != ""
                     && (r.EndDate == null || r.EndDate > now))
            .OrderByDescending(r => r.IsPinned)
            .OrderByDescending(r => r.CreatedAt)
            .Take(3)
            .ToList();

        // 3. 右侧列表
        // 3a. 文字推荐（无图片、未过期）
        var textRecs = _db.Queryable<Recommendation>()
            .Where(r => (r.ImageUrl == null || r.ImageUrl == "")
                     && (r.EndDate == null || r.EndDate > now))
            .OrderByDescending(r => r.IsPinned)
            .OrderByDescending(r => r.CreatedAt)
            .ToList()
            .Select(r => new HomeListItem
            {
                Title = r.Title,
                Url = r.LinkUrl,
                SourceType = "推荐",
                ShowDate = false
            }).ToList();

        ListItems.AddRange(textRecs);

        // 3b. 如果不足8条，从攻略和资讯里按添加时间倒序补全
        int remaining = 8 - ListItems.Count;
        if (remaining > 0)
        {
            // 攻略（guide 分类，IsHome=true，未屏蔽）
            var travelogues = _db.Queryable<Travelogue>()
                .Where(t => t.IsHome && !t.IsHidden && t.Classification == "guide")
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new { t.Id, t.Title, t.CreatedAt, Type = "攻略" })
                .ToList();

            // 资讯（IsHome=true，未屏蔽）
            var news = _db.Queryable<TravelPortal.Web.Models.News>()
                .Where(n => n.IsHome && !n.IsHidden)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new { n.Id, n.Title, n.CreatedAt, Type = "资讯" })
                .ToList();

            // 合并按添加时间倒序取 remaining 条
            var combined = travelogues
                .Select(t => new HomeListItem
                {
                    Title = t.Title,
                    Url = $"/Travelogues/{t.Id}",
                    SourceType = t.Type,
                    ShowDate = true,
                    CreatedAt = t.CreatedAt
                })
                .Concat(news.Select(n => new HomeListItem
                {
                    Title = n.Title,
                    Url = $"/News/{n.Id}",
                    SourceType = n.Type,
                    ShowDate = true,
                    CreatedAt = n.CreatedAt
                }))
                .OrderByDescending(x => x.CreatedAt)
                .Take(remaining)
                .ToList();

            ListItems.AddRange(combined);
        }
    }
}

/// <summary>首页右侧列表条目（统一结构）</summary>
public class HomeListItem
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty; // 推荐/攻略/资讯
    public bool ShowDate { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}
