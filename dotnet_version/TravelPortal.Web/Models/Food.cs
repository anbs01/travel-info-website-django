using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 特产美食模型
/// </summary>
[SugarTable("tp_foods")]
public class Food : BaseContent
{
    /// <summary>
    /// 产品类型（美食、特产）
    /// </summary>
    [SugarColumn(Length = 50)]
    public string ProductType { get; set; } = "美食";

    /// <summary>
    /// 特产级别（无公害产品、绿色产品、有机产品、地理标志产品）
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? SpecialtyLevel { get; set; }

    /// <summary>
    /// 子分类（菜系 或 特产类别）
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? Classification { get; set; }

    /// <summary>
    /// 非遗级别（国家级、省级、市级、县级）
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? NonLegacyLevel { get; set; }

    // --- 导航属性 ---

    [SugarColumn(IsIgnore = true)]
    public Geo? Geo { get; set; }
}
