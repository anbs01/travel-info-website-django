using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.tpco.System
{
    [IgnoreAntiforgeryToken]
    public class PasswordModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        // 密码规则：至少 8 位，包含大小写字母和数字
        private static readonly Regex PasswordRegex = new Regex(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            RegexOptions.Compiled);

        public PasswordModel(ISqlSugarClient db)
        {
            _db = db;
        }

        public void OnGet() { }

        /// <summary>
        /// AJAX 提交：返回 JSON 而非整页刷新，保留表单状态
        /// </summary>
        public async Task<IActionResult> OnPostAsync([FromBody] PasswordRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.OldPassword))
                return new JsonResult(new { ok = false, field = "old", msg = "请输入当前密码。" });

            if (string.IsNullOrWhiteSpace(req.NewPassword))
                return new JsonResult(new { ok = false, field = "new", msg = "请输入新密码。" });

            if (!PasswordRegex.IsMatch(req.NewPassword))
                return new JsonResult(new { ok = false, field = "new", msg = "新密码需至少 8 位，且包含大写字母、小写字母和数字。" });

            if (req.NewPassword != req.ConfirmPassword)
                return new JsonResult(new { ok = false, field = "confirm", msg = "两次输入的新密码不一致。" });

            // 获取当前用户
            var userIdStr = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return new JsonResult(new { ok = false, field = "old", msg = "登录已过期，请重新登录。" });

            var user = await _db.Queryable<SysUser>().InSingleAsync(userId);
            if (user == null)
                return new JsonResult(new { ok = false, field = "old", msg = "当前用户不存在，请重新登录。" });

            // 原密码校验
            if (!BCrypt.Net.BCrypt.Verify(req.OldPassword, user.PasswordHash))
                return new JsonResult(new { ok = false, field = "old", msg = "原密码输入有误，请重新输入。" });

            // 防止新旧密码相同
            if (BCrypt.Net.BCrypt.Verify(req.NewPassword, user.PasswordHash))
                return new JsonResult(new { ok = false, field = "new", msg = "新密码不能与当前密码相同。" });

            // 更新密码
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            await _db.Updateable(user).UpdateColumns(it => it.PasswordHash).ExecuteCommandAsync();

            return new JsonResult(new { ok = true, msg = "密码已成功修改！下次登录请使用新密码。" });
        }

        /// <summary>AJAX 请求体结构</summary>
        public class PasswordRequest
        {
            public string OldPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
            public string ConfirmPassword { get; set; } = string.Empty;
        }
    }
}
