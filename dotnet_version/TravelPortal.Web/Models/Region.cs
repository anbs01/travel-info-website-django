using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 地区（省份 / 国家）模型
/// </summary>
[SugarTable("tp_regions")]
public class Region
{
    /// <summary>
    /// 主键 ID
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 地区简称
    /// </summary>
    [SugarColumn(Length = 100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 地区全称，用于面包屑和详情
    /// </summary>
    [SugarColumn(Length = 200)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// 介绍内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? Introduction { get; set; }

    /// <summary>
    /// 是否海外
    /// </summary>
    public bool IsOverseas { get; set; } = false;

    /// <summary>
    /// 排序权重，默认 0
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 系统创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
