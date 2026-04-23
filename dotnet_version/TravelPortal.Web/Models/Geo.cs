using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 地理层级模型（国家、省、市、县、乡、村）
/// </summary>
[SugarTable("tp_geo")]
public class Geo : BaseContent
{
    /// <summary>
    /// 父级 ID（顶层为 0 或 NULL）
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 层级：1=国家, 2=省, 3=市, 4=县, 5=乡镇, 6=村
    /// </summary>
    public int Level { get; set; }

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
    /// 城乡代码（行政区划代码）
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string? GeoCode { get; set; }

    /// <summary>
    /// 城乡标签（市府所在地、省会城市等）
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? GeoTag { get; set; }

    /// <summary>
    /// 最佳旅游/居住时间
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? BestTime { get; set; }

    /// <summary>
    /// 英文名称（海外城镇使用）
    /// </summary>
    [SugarColumn(Length = 200, IsNullable = true)]
    public string? EnglishName { get; set; }

    /// <summary>
    /// 辖区层级（省份使用，多选，存逗号隔开字符串）
    /// </summary>
    [SugarColumn(Length = 200, IsNullable = true)]
    public string? JurisdictionLayers { get; set; }

    // --- 导航属性 ---

    [SugarColumn(IsIgnore = true)]
    public Geo? Parent { get; set; }

    [SugarColumn(IsIgnore = true)]
    public List<Geo> Children { get; set; } = new();
}
