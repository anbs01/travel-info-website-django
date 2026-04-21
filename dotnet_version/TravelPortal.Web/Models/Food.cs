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
    [SugarColumn(Length = 50)]
    public string Category { get; set; } = "美食";

    /// <summary>
    /// 美食菜系
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? Cuisine { get; set; }

    /// <summary>
    /// 特产分类
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? SpecialtyCategory { get; set; }

    /// <summary>
    /// 特产级别
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? SpecialtyLevel { get; set; }

    /// <summary>
    /// 非遗级别
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? HeritageLevel { get; set; }

    /// <summary>
    /// 口味特色（120字以内）
    /// </summary>
    [SugarColumn(Length = 500, IsNullable = true)]
    public string? TasteFeatures { get; set; }

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
    /// 详细介绍内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 关联地区（仅用于展示，不映射到数据库）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public Region? Region { get; set; }
}
