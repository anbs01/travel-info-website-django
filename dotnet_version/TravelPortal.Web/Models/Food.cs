using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 特产美食模型
/// </summary>
[SugarTable("tp_foods")]
public class Food : BaseContent
{
    /// <summary>
    /// 产品分类（美食 / 特产）
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string? Category { get; set; }

    /// <summary>
    /// 美食菜系（从热词中显示）
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? Cuisine { get; set; }

    /// <summary>
    /// 特产分类（从特产数据中显示）
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? SpecialtyCategory { get; set; }

    /// <summary>
    /// 特产级别（无公害产品、绿色产品、有机产品、地理标志产品）
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? SpecialtyLevel { get; set; }

    /// <summary>
    /// 非遗级别（国际家非遗、省级非遗、市级非遗、县级非遗）
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? IntangibleHeritageLevel { get; set; }

    /// <summary>
    /// 所属城镇 ID
    /// </summary>
    public int? PlaceId { get; set; }

    /// <summary>
    /// 详细介绍内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 价格 / 参考人均
    /// </summary>
    [SugarColumn(DecimalDigits = 2)]
    public decimal? Price { get; set; }
}
