using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 景区 / 打卡点模型
/// </summary>
[SugarTable("tp_scenic_spots")]
public class ScenicSpot : BaseContent
{
    /// <summary>
    /// 所属城镇 ID
    /// </summary>
    public int PlaceId { get; set; }

    /// <summary>
    /// 类型标记（scenic:景区 / checkin:打卡点）
    /// </summary>
    public string SpotType { get; set; } = "scenic";
}
