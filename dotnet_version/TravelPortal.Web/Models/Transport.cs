using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 交通点模型
/// </summary>
[SugarTable("tp_transports")]
public class Transport : BaseContent
{
    /// <summary>
    /// 关联的地理节点 ID
    /// </summary>
    public int GeoId { get; set; }

    /// <summary>
    /// 交通类型：Airport(机场), Railway(火车站), Bus(汽车站), Pier(码头)
    /// </summary>
    [SugarColumn(Length = 50)]
    public string TransportType { get; set; } = "Railway";

    /// <summary>
    /// 详细地址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; }

    // --- 导航属性 ---
    [SugarColumn(IsIgnore = true)]
    public Geo? Geo { get; set; }
}
