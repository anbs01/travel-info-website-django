using SqlSugar;

namespace TravelPortal.Web.Models
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    [SugarTable("tp_sys_user")]
    public class SysUser
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(ColumnDescription = "用户名", Length = 50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 密码哈希 (BCrypt)
        /// </summary>
        [SugarColumn(ColumnDescription = "密码哈希", Length = 255)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(ColumnDescription = "昵称", Length = 50)]
        public string? Nickname { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(ColumnDescription = "头像", Length = 255, IsNullable = true)]
        public string? Avatar { get; set; }

        /// <summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
