using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 基础物理实体
/// </summary>
public abstract class BaseEntity
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 基础内容实体（对应文档中的 BaseContent）
/// </summary>
public abstract class BaseContent : BaseEntity
{
    [SugarColumn(Length = 200)]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(ColumnDescription = "SEO简介", ColumnDataType = "text")]
    public string Summary { get; set; } = string.Empty;

    [SugarColumn(ColumnDescription = "主图")]
    public string MainImage { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;
}

/// <summary>
/// 2.1 Region（行政区/国家）
/// </summary>
[SugarTable("Regions")]
public class Region : BaseEntity
{
    [SugarColumn(Length = 50)]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(Length = 100)]
    public string FullName { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;
}

/// <summary>
/// 2.2 Place（城镇乡村）
/// </summary>
[SugarTable("Places")]
public class Place : BaseContent
{
    public int RegionId { get; set; }

    [SugarColumn(Length = 100, IsNullable = false)]
    public string EnglishCode { get; set; } = string.Empty;

    [SugarColumn(Length = 100)]
    public string Alias { get; set; } = string.Empty;

    [SugarColumn(Length = 200)]
    public string BestTime { get; set; } = string.Empty;

    public string PlaceType { get; set; } = string.Empty; // 地级市/县级市/乡镇/村庄

    public bool IsOverseas { get; set; } = false;
    public bool HideNav { get; set; } = false;
}

/// <summary>
/// 2.3 ScenicSpot（景区 / 打卡点）
/// </summary>
[SugarTable("ScenicSpots")]
public class ScenicSpot : BaseContent
{
    public int PlaceId { get; set; }
    public string SpotType { get; set; } = string.Empty; // scenic/checkin
}

/// <summary>
/// 2.4 Food（美食 / 文创）
/// </summary>
[SugarTable("Foods")]
public class Food : BaseContent
{
    public int PlaceId { get; set; }
    public string Category { get; set; } = string.Empty; // food/culture
}

/// <summary>
/// 2.5 Travelogue（游记 / 攻略）
/// </summary>
[SugarTable("Travelogues")]
public class Travelogue : BaseEntity
{
    [SugarColumn(Length = 200)]
    public string Title { get; set; } = string.Empty;

    [SugarColumn(Length = 100)]
    public string Slug { get; set; } = string.Empty;

    [SugarColumn(ColumnDataType = "text")]
    public string Content { get; set; } = string.Empty;

    public string CoverImage { get; set; } = string.Empty;

    public int Views { get; set; } = 0;
}
