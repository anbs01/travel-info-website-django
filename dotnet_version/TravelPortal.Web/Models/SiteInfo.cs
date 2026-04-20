using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 网站基本信息配置
/// </summary>
[SugarTable("tp_site_info")]
public class SiteInfo
{
    /// <summary>
    /// 主键 ID（通常为单例 1）
    /// </summary>
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; } = 1;

    /// <summary>
    /// 关于我们内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? About { get; set; }

    /// <summary>
    /// 服务协议内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? ServiceAgreement { get; set; }

    /// <summary>
    /// 合作事宜内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? Cooperation { get; set; }

    /// <summary>
    /// 联系我们内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? ContactUs { get; set; }

    /// <summary>
    /// 页脚版权声明文字
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? Copyright { get; set; }

    /// <summary>
    /// 原创版权内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? OriginalCopyright { get; set; }

    /// <summary>
    /// 转载声明内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? ReprintStatement { get; set; }

    /// <summary>
    /// 广告提示内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? AdDisclaimer { get; set; }

    /// <summary>
    /// 风险提示内容
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? RiskWarning { get; set; }
}
