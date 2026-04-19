using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace TravelPortal.Web.Models;

/// <summary>
/// 公共抽象基类 - 对应文档 1.1 BaseContent
/// </summary>
public abstract class BaseContent
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 简称/短标题
    /// </summary>
    [SugarColumn(Length = 200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 全称 (用于详情页)
    /// </summary>
    [SugarColumn(Length = 200, IsNullable = true)]
    public string? FullTitle { get; set; }

    /// <summary>
    /// 详情页导语/特色说明
    /// </summary>
    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? Summary { get; set; }

    /// <summary>
    /// [灵魂] 品别/级别标签 (如：非遗级别、菜系)
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string? LevelTag { get; set; }

    /// <summary>
    /// [灵魂] 特色标签云 (逗号隔开)
    /// </summary>
    [SugarColumn(Length = 255, IsNullable = true)]
    public string? FeatureTags { get; set; }

    /// <summary>
    /// 焦点图
    /// </summary>
    [SugarColumn(Length = 500, IsNullable = true)]
    public string? MainImage { get; set; }

    /// <summary>
    /// 前台显示的自定义发布日期
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public DateTime? PublishDate { get; set; }

    /// <summary>
    /// 内容来源
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? Source { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? Author { get; set; }

    /// <summary>
    /// 阅读量
    /// </summary>
    public int Views { get; set; } = 0;

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsSticky { get; set; } = false;

    /// <summary>
    /// 置顶时间
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
    /// 系统创建时间
    /// </summary>
    [SugarColumn(IsOnlyIgnoreUpdate = true)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 系统更新时间
    /// </summary>
    [SugarColumn(IsOnlyIgnoreInsert = true)]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 2.1 地区（省份/国家）
/// </summary>
[SugarTable("tp_regions")]
public class Region
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    [SugarColumn(Length = 100)]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(Length = 200)]
    public string FullName { get; set; } = string.Empty;

    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? Introduction { get; set; }

    public bool IsOverseas { get; set; } = false;

    public int SortOrder { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 2.2 城镇乡村
/// </summary>
[SugarTable("tp_places")]
public class Place : BaseContent
{
    public int RegionId { get; set; }

    /// <summary>
    /// 纯英文代码，用于构成 URL
    /// </summary>
    [SugarColumn(Length = 100, IndexGroupNameList = new string[] { "index_english_code" })]
    public string EnglishCode { get; set; } = string.Empty;

    public string? Alias { get; set; }
    public string? BestTime { get; set; }

    /// <summary>
    /// 地级市 / 乡镇 / 村庄
    /// </summary>
    public string? PlaceType { get; set; }

    public bool IsOverseas { get; set; } = false;
    public int SortOrder { get; set; } = 100;
}

/// <summary>
/// 2.3 景区 / 打卡点
/// </summary>
[SugarTable("tp_scenic_spots")]
public class ScenicSpot : BaseContent
{
    public int PlaceId { get; set; }

    /// <summary>
    /// scenic（景区）/ checkin（打卡点）
    /// </summary>
    public string SpotType { get; set; } = "scenic";
}

/// <summary>
/// 3.3 美食
/// </summary>
[SugarTable("tp_foods")]
public class Food : BaseContent
{
    public int? PlaceId { get; set; }

    [SugarColumn(ColumnDataType = "text")]
    public string Content { get; set; } = string.Empty;

    [SugarColumn(DecimalDigits = 2)]
    public decimal? Price { get; set; }
}

/// <summary>
/// 3.2 攻略/游记
/// </summary>
[SugarTable("tp_travelogues")]
public class Travelogue : BaseContent
{
    public int? PlaceId { get; set; }

    /// <summary>
    /// 发布时生成的 YYYYMMDDHHmm
    /// </summary>
    [SugarColumn(Length = 20, IndexGroupNameList = new string[] { "index_slug" })]
    public string Slug { get; set; } = string.Empty;

    [SugarColumn(ColumnDataType = "text")]
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// 4.3 网站基本信息 (单例)
/// </summary>
[SugarTable("tp_site_info")]
public class SiteInfo
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; } = 1;

    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? About { get; set; }

    [SugarColumn(ColumnDataType = "text", IsNullable = true)]
    public string? Copyright { get; set; }
}
