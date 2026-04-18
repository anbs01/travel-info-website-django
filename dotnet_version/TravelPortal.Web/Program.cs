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

    // 自动建表 (CodeFirst)
    if (builder.Environment.IsDevelopment())
    {
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
