using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 景区 / 打卡点模型
/// </summary>
[SugarTable("tp_scenic_spots")]
public class ScenicSpot : BaseContent
{
    /// <summary>
    /// 名气级别（知名景区、热门景点、小众秘境、城市地标、网红打卡地）
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string? FameLevel { get; set; }

    /// <summary>
    /// 景区级别（AAAAA级、AAAA级、AAA级、AA级、A级）
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string? ScenicGrade { get; set; }

    // --- 导航属性 ---

    [SugarColumn(IsIgnore = true)]
    public Geo? Geo { get; set; }
}
