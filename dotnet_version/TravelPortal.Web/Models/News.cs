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
    public string Category { get; set; } = "news";

    /// <summary>
    /// 文章摘要（如果为空则取正文前100字）
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? Excerpt { get; set; }

    /// <summary>
    /// 正文内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text")]
    public string Content { get; set; } = string.Empty;
}
