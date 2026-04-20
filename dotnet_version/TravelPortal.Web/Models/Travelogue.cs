using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 纪行攻略模型
/// </summary>
[SugarTable("tp_travelogues")]
public class Travelogue : BaseContent
{
    /// <summary>
    /// 所属城镇 ID
    /// </summary>
    public int? PlaceId { get; set; }

    /// <summary>
    /// 发布类型（travelogue:纪行 / guide:攻略）
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Category { get; set; } = "travelogue";

    /// <summary>
    /// 发布标识（格式：YYYYMMDDHHmm）
    /// </summary>
    [SugarColumn(Length = 20, IndexGroupNameList = new string[] { "index_slug" })]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// 选中的类别标签（逗号隔开，如：摄影,自驾）
    /// </summary>
    [SugarColumn(Length = 500, IsNullable = true)]
    public string? Tags { get; set; }

    /// <summary>
    /// 正文详细内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text")]
    public string Content { get; set; } = string.Empty;
}
