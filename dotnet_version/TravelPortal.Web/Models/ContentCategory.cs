using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 内容与分类（热词）的多对多中间表
/// </summary>
[SugarTable("tp_content_categories")]
public class ContentCategory
{
    /// <summary>
    /// 主键 ID
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 业务实体 ID
    /// </summary>
    public int ContentId { get; set; }

    /// <summary>
    /// 分类（热词）ID
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// 业务模块标识（News, Food, Scenic, Travelogue, Creative）
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Module { get; set; } = string.Empty;
}
