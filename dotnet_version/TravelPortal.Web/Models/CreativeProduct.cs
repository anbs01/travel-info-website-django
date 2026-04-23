using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 文创产品模型
/// </summary>
[SugarTable("tp_creative_products")]
public class CreativeProduct : BaseContent
{
    /// <summary>
    /// 产品分类
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
