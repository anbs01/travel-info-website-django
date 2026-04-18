using SqlSugar;
using TravelPortal.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// 注册 SqlSugar
builder.Services.AddScoped<ISqlSugarClient>(s =>
{
    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
    {
        ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection"),
        DbType = DbType.MySql,
        IsAutoCloseConnection = true,
        InitKeyType = InitKeyType.Attribute
    });

    // 自动建库建表 (CodeFirst)
    if (builder.Environment.IsDevelopment())
    {
        // 1. 尝试创建数据库（如果不存在）
        db.DbMaintenance.CreateDatabase();

        // 2. 尝试给普通用户授权（如果是 root 登录）
        try {
            db.Ado.ExecuteCommand("GRANT ALL PRIVILEGES ON TravelPortal_DB.* TO 'travel_user'@'%';");
            db.Ado.ExecuteCommand("FLUSH PRIVILEGES;");
        } catch { /* 如果非 root 登录或用户已存在权限，忽略报错 */ }

        // 3. 初始化表
        db.CodeFirst.InitTables(
            typeof(Region), 
            typeof(Place), 
            typeof(ScenicSpot), 
            typeof(Food), 
            typeof(Travelogue)
        );
    }

    return db;
});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
