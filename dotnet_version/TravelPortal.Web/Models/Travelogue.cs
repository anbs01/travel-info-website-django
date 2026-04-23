using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 纪行攻略模型
/// </summary>
[SugarTable("tp_travelogues")]
public class Travelogue : BaseContent
{
    /// <summary>
    /// 作品分类（纪行、攻略）
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Classification { get; set; } = "纪行";

    // --- 导航属性 ---

    [SugarColumn(IsIgnore = true)]
    public Geo? Geo { get; set; }
}
