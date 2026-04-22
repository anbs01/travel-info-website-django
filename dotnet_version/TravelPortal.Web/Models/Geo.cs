using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 地理层级模型（国家、省、市、县、乡、村）
/// </summary>
[SugarTable("tp_geo")]
public class Geo
{
    /// <summary>
    /// 主键 ID
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 父级 ID（顶层为 0 或 NULL）
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 层级：1=国家, 2=省, 3=市, 4=县, 5=乡镇, 6=村
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 节点名称
    /// </summary>
    [SugarColumn(Length = 200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 拼音/英文代码，用于 URL
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? Slug { get; set; }

    /// <summary>
    /// 性质：Domestic(国内), Overseas(海外)
    /// </summary>
    [SugarColumn(Length = 20)]
    public string Nature { get; set; } = "Domestic";

    /// <summary>
    /// 祖先路径，格式如 "1/10/20"
    /// </summary>
    [SugarColumn(Length = 500, IsNullable = true)]
    public string? AncestorPath { get; set; }

    /// <summary>
    /// 介绍内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? Introduction { get; set; }

    /// <summary>
    /// 最佳旅游时间
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? BestTime { get; set; }

    /// <summary>
    /// 是否隐藏
    /// </summary>
    public bool IsHidden { get; set; } = false;

    /// <summary>
    /// 排序权重
    /// </summary>
    public int SortOrder { get; set; } = 100;

    /// <summary>
    /// 系统创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // --- 导航属性 ---

    [SugarColumn(IsIgnore = true)]
    public Geo? Parent { get; set; }

    [SugarColumn(IsIgnore = true)]
    public List<Geo> Children { get; set; } = new();
}
