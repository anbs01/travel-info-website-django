using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 行业资讯模型
/// </summary>
[SugarTable("tp_news")]
public class News : BaseContent
{
    /// <summary>
    /// 资讯分类（news:行业资讯 / notice:官方公告）
    /// </summary>
    [SugarColumn(Length = 50)]
    public string NewsCategory { get; set; } = "news";
}
