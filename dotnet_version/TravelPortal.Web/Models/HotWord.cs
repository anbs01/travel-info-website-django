using SqlSugar;

namespace TravelPortal.Web.Models;

/// <summary>
/// 类别热词管理模型
/// </summary>
[SugarTable("tp_hot_words")]
public class HotWord
{
    /// <summary>
    /// 主键 ID
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 热词名称
    /// </summary>
    [SugarColumn(Length = 100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 是否显示在：首页搜索词
    /// </summary>
    public bool ShowInHome { get; set; } = false;

    /// <summary>
    /// 是否显示在：城乡搜索词
    /// </summary>
    public bool ShowInPlace { get; set; } = false;

    /// <summary>
    /// 是否显示在：打卡地类别
    /// </summary>
    public bool ShowInScenic { get; set; } = false;

    /// <summary>
    /// 是否显示在：纪行类别
    /// </summary>
    public bool ShowInTravelogue { get; set; } = false;

    /// <summary>
    /// 是否显示在：攻略类别
    /// </summary>
    public bool ShowInGuide { get; set; } = false;

    /// <summary>
    /// 是否显示在：特产类别
    /// </summary>
    public bool ShowInSpecialty { get; set; } = false;

    /// <summary>
    /// 是否显示在：美食菜系
    /// </summary>
    public bool ShowInCuisine { get; set; } = false;

    /// <summary>
    /// 是否显示在：文创类别
    /// </summary>
    public bool ShowInCreative { get; set; } = false;

    /// <summary>
    /// 是否显示在：资讯类别
    /// </summary>
    public bool ShowInNews { get; set; } = false;

    /// <summary>
    /// 排序权重
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 是否屏蔽（隐藏）
    /// </summary>
    public bool IsHidden { get; set; } = false;

    /// <summary>
    /// 系统创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
