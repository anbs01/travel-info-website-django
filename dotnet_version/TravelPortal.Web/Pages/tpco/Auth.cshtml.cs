using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.tpco
{
    public class AuthModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly SqlSugar.ISqlSugarClient _db;

        public AuthModel(SqlSugar.ISqlSugarClient db)
        {
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "请输入用户名")]
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "请输入密码")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/tpco");

            if (!ModelState.IsValid) return Page();

            var user = await _db.Queryable<SysUser>()
                .FirstAsync(u => u.Username == Input.Username);

            if (user == null || user.IsDisabled || !BCrypt.Net.BCrypt.Verify(Input.Password, user.PasswordHash))
            {
                ErrorMessage = "用户名或密码错误";
                return Page();
            }

            user.LastLoginTime = DateTime.Now;
            await _db.Updateable(user).UpdateColumns(u => u.LastLoginTime).ExecuteCommandAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Nickname", user.Nickname ?? user.Username),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Avatar", user.Avatar ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = Input.RememberMe });

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/tpco/Auth");
        }
    }
}
