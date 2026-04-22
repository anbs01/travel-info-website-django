using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

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
builder.Services.AddRazorPages();

var app = builder.Build();

// 数据库自动维护 (仅 root 模式，极简逻辑)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
        try 
        {
            // 1. 创建数据库 (SqlSugar 会自动处理连接字符串中不带库名的情况)
            db.DbMaintenance.CreateDatabase();
            
            // 2. 初始化所有业务表
            db.CodeFirst.InitTables(
                typeof(Geo), 
                typeof(Transport), 
                typeof(ScenicSpot), 
                typeof(Food), 
                typeof(Travelogue),
                typeof(News),
                typeof(SiteInfo),
                typeof(HotWord),
                typeof(Recommendation),
                typeof(CreativeProduct)
            );
            Console.WriteLine("✅ [Root Mode] 数据库初始化成功！");
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
app.UseAuthorization();
app.MapRazorPages();

app.Run();
