using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 内容公共抽象基类
/// </summary>
public abstract class BaseContent
{
    /// <summary>
    /// 主键 ID
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 简称/短标题，用于列表、版面受限处
    /// </summary>
    [SugarColumn(Length = 200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 全称，用于详情页及版面宽敞处（对应资讯引题等）
    /// </summary>
    [SugarColumn(Length = 200, IsNullable = true)]
    public string? FullTitle { get; set; }

    /// <summary>
    /// 副题/别名（对应资讯副题、城乡别名等）
    /// </summary>
    [SugarColumn(Length = 200, IsNullable = true)]
    public string? SubTitle { get; set; }

    /// <summary>
    /// 摘要/打卡特色/产品特色（120字以内）
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? Summary { get; set; }

    /// <summary>
    /// 正文内容（富文本编辑器）
    /// </summary>
    [SugarColumn(ColumnDataType = "longtext", IsNullable = true)]
    public string? Content { get; set; }

    /// <summary>
    /// 品别/级别标签（如：非遗级别、菜系）
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string? LevelTag { get; set; }

    /// <summary>
    /// SEO 友好链接别名
    /// </summary>
    [SugarColumn(Length = 200, IsNullable = true)]
    public string? Slug { get; set; }

    /// <summary>
    /// 特色标签云（如：亲子,观海），逗号隔开
    /// </summary>
    [SugarColumn(Length = 255, IsNullable = true)]
    public string? Tags { get; set; }

    /// <summary>
    /// 焦点图地址（支持从正文提取或手动上传）
    /// </summary>
    [SugarColumn(Length = 500, IsNullable = true)]
    public string? MainImage { get; set; }

    /// <summary>
    /// 前台显示的自定义发布日期（默认当前）
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public DateTime? PublishDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 内容来源
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? Source { get; set; }

    /// <summary>
    /// 作者 / 贡献者
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? Author { get; set; }

    /// <summary>
    /// 原文链接
    /// </summary>
    [SugarColumn(Length = 500, IsNullable = true)]
    public string? OriginUrl { get; set; }

    /// <summary>
    /// 跳转链接，填写后直接跳转外部页
    /// </summary>
    [SugarColumn(Length = 500, IsNullable = true)]
    public string? RedirectUrl { get; set; }

    /// <summary>
    /// 关联地区 ID
    /// </summary>
    public int? GeoId { get; set; }

    /// <summary>
    /// 版权类型（如：原创、转载等）
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string? CopyrightType { get; set; }

    /// <summary>
    /// 是否显示水印
    /// </summary>
    public bool ShowWatermark { get; set; } = false;

    /// <summary>
    /// 水印位置
    /// </summary>
    [SugarColumn(Length = 20, IsNullable = true)]
    public string? WatermarkPos { get; set; }

    /// <summary>
    /// 是否显示详情页导航条
    /// </summary>
    public bool ShowNavStrip { get; set; } = false;

    /// <summary>
    /// 阅读量
    /// </summary>
    public int Views { get; set; } = 0;

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsSticky { get; set; } = false;

    /// <summary>
    /// 置顶操作时间
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public DateTime? StickyAt { get; set; }

    /// <summary>
    /// 是否推荐至首页
    /// </summary>
    public bool IsHome { get; set; } = false;

    /// <summary>
    /// 是否隐藏
    /// </summary>
    public bool IsHidden { get; set; } = false;

    /// <summary>
    /// 排序权重
    /// </summary>
    public int SortOrder { get; set; } = 100;

    /// <summary>
    /// 系统添加时间
    /// </summary>
    [SugarColumn(IsOnlyIgnoreUpdate = true)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
