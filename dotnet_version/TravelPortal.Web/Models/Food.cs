using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 特产美食模型
/// </summary>
[SugarTable("tp_foods")]
public class Food : BaseContent
{
    /// <summary>
    /// 产品分类（如：地方特产, 招牌菜品）
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string? Category { get; set; }

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
