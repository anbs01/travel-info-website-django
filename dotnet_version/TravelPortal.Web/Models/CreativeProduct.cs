using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 文创产品模型
/// </summary>
[SugarTable("tp_creative_products")]
public class CreativeProduct : BaseContent
{
    /// <summary>
    /// 产品类别
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 关联地区 ID
    /// </summary>
    public int? RegionId { get; set; }

    /// <summary>
    /// 原文链接
    /// </summary>
    [SugarColumn(Length = 500, IsNullable = true)]
    public string? OriginalUrl { get; set; }

    /// <summary>
    /// 正文内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text")]
    public string Content { get; set; } = string.Empty;
}
