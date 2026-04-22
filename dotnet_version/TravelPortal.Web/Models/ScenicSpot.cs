using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 景区 / 打卡点模型
/// </summary>
[SugarTable("tp_scenic_spots")]
public class ScenicSpot : BaseContent
{
    /// <summary>
    /// 关联地理节点 ID
    /// </summary>
    public int GeoId { get; set; }

    /// <summary>
    /// 类型标记（scenic:景区 / checkin:打卡点）
    /// </summary>
    public string SpotType { get; set; } = "scenic";

    /// <summary>
    /// 关联地理节点
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public Geo? Geo { get; set; }
}
