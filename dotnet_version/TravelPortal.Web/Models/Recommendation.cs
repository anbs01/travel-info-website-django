using SqlSugar;

namespace TravelPortal.Web.Models
{
    /// <summary>
    /// 首页推荐数据模型
    /// </summary>
    [SugarTable("tp_recommendations")]
    public class Recommendation
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 推荐标题
        /// </summary>
        [SugarColumn(Length = 200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 焦点图 (不上传则为文字推荐)
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [SugarColumn(Length = 500)]
        public string LinkUrl { get; set; } = string.Empty;

        /// <summary>
        /// 点击数
        /// </summary>
        public int ClickCount { get; set; } = 0;

        /// <summary>
        /// 置顶状态
        /// </summary>
        public bool IsPinned { get; set; } = false;

        /// <summary>
        /// 排序权重 (置顶逻辑使用)
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// 添加日期
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 结束日期 (不输入表示一直有效)
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 计算状态：正显示 / 已到期
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string Status => (EndDate.HasValue && EndDate < DateTime.Now) ? "已到期" : "正显示";
    }
}
