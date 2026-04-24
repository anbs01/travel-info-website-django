using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 类别热词管理模型
/// </summary>
[SugarTable("tp_hot_words")]
public class HotWord
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 热词名称
    /// </summary>
    [SugarColumn(Length = 100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 所属模块：home / place / scenic / travelogue / guide / specialty / cuisine / creative / news
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// 排序权重
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 是否屏蔽
    /// </summary>
    public bool IsHidden { get; set; } = false;

    /// <summary>
    /// 系统创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ── 模块常量，避免魔法字符串 ──────────────────────────
    public const string MOD_HOME       = "home";
    public const string MOD_PLACE      = "place";
    public const string MOD_SCENIC     = "scenic";
    public const string MOD_TRAVELOGUE = "travelogue";
    public const string MOD_GUIDE      = "guide";
    public const string MOD_SPECIALTY  = "specialty";
    public const string MOD_CUISINE    = "cuisine";
    public const string MOD_CREATIVE   = "creative";
    public const string MOD_NEWS       = "news";
}
