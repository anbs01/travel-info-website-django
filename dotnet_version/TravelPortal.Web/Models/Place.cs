using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 城镇乡村模型
/// </summary>
[SugarTable("tp_places")]
public class Place : BaseContent
{
    /// <summary>
    /// 所属地区 ID
    /// </summary>
    public int RegionId { get; set; }

    /// <summary>
    /// 英文代码，用于构成 URL 路径
    /// </summary>
    [SugarColumn(Length = 100, IndexGroupNameList = new string[] { "index_english_code" })]
    public string EnglishCode { get; set; } = string.Empty;

    /// <summary>
    /// 别名
    /// </summary>
    public string? Alias { get; set; }

    /// <summary>
    /// 最佳旅游季节/时间
    /// </summary>
    public string? BestTime { get; set; }

    /// <summary>
    /// 地点级别（如：地级市 / 乡镇 / 村庄）
    /// </summary>
    public string? PlaceType { get; set; }

    /// <summary>
    /// 是否海外
    /// </summary>
    public bool IsOverseas { get; set; } = false;

    /// <summary>
    /// 排序权重
    /// </summary>
    public int SortOrder { get; set; } = 100;
}
