using Microsoft.AspNetCore.Authentication.Cookies;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;
// 注册编码提供程序以支持 GB2312 (拼音识别需要)
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// 注册 SqlSugar
builder.Services.AddScoped<ISqlSugarClient>(s =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlSugarClient(new ConnectionConfig()
    {
        ConnectionString = connectionString,
        DbType = DbType.MySql,
        IsAutoCloseConnection = true,
        InitKeyType = InitKeyType.Attribute
    });
});

builder.Services.AddScoped<IUploadService, UploadService>();

// 配置认证与授权
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/tpco/Auth"; // 登录路径
        options.AccessDeniedPath = "/tpco/Auth"; // 拒绝访问路径
        options.Cookie.Name = ".TravelPortal.Auth";
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Cookie 有效期
    });

builder.Services.AddAuthorization();

builder.Services.AddRazorPages(options =>
{
    // 强制锁定 /tpco 文件夹，必须登录才能访问
    options.Conventions.AuthorizeFolder("/tpco");
    // 允许匿名访问登录页
    options.Conventions.AllowAnonymousToPage("/tpco/Auth");
});

var app = builder.Build();

// 数据库自动维护 (仅 root 模式，极简逻辑)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
        try 
        {
            // 1. 创建数据库
            db.DbMaintenance.CreateDatabase();
            
            // 2. 初始化所有业务表
            db.CodeFirst.InitTables(
                typeof(SysUser), // 新增系统用户表
                typeof(Geo), 
                typeof(Transport), 
                typeof(ScenicSpot), 
                typeof(Food), 
                typeof(Travelogue),
                typeof(News),
                typeof(SiteInfo),
                typeof(HotWord),
                typeof(Recommendation),
                typeof(CreativeProduct),
                typeof(ContentCategory)
            );
            Console.WriteLine("✅ [Root Mode] 数据库初始化成功！");

            // 3. 初始化超级管理员账号
            if (!db.Queryable<SysUser>().Any())
            {
                var adminUser = new SysUser
                {
                    Username = "admin",
                    // 默认密码: admin123 (使用 BCrypt 加密)
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Nickname = "超级管理员",
                    CreateTime = DateTime.Now
                };
                db.Insertable(adminUser).ExecuteCommand();
                Console.WriteLine("🚀 [Init] 已创建默认管理员账号: admin / admin123 (请及时修改密码)");
            }

            // 4. 自动补全演示数据主图
            if (db.Queryable<Travelogue>().Any() && !db.Queryable<Travelogue>().Any(t => t.MainImage != null))
            {
                var list = db.Queryable<Travelogue>().OrderBy(t => t.Id).Take(4).ToList();
                string[] demoImages = {
                    "https://images.unsplash.com/photo-1540660290370-8af90a454417?w=800",
                    "https://images.unsplash.com/photo-1527685238219-c81b3dd33021?w=800",
                    "https://images.unsplash.com/photo-1508804185872-d7badad00f7d?w=800",
                    "https://images.unsplash.com/photo-1444723121867-7a241cacace9?w=800"
                };
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].MainImage = demoImages[i];
                }
                db.Updateable(list).UpdateColumns(it => it.MainImage).ExecuteCommand();
                Console.WriteLine("✨ [Data Fix] 已自动为前 4 条纪行攻略补全演示图！");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 数据库维护失败: {ex.Message}");
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // 必须在 UseAuthorization 之前
app.UseAuthorization();

app.MapRazorPages();

app.Run();
