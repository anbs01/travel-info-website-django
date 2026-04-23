using System;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Scratch;

public class SyncDbSchema
{
    public static void Run()
    {
        var db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Server=127.0.0.1;Port=3308;Database=TravelPortal_DB;Uid=root;Pwd=root_password_2026;Character Set=utf8mb4;",
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });

        Console.WriteLine("🚀 开始同步数据库表结构...");

        try
        {
            db.CodeFirst.InitTables(
                typeof(Geo), 
                typeof(Transport), 
                typeof(ScenicSpot), 
                typeof(Food), 
                typeof(Travelogue),
                typeof(News),
                typeof(HotWord),
                typeof(CreativeProduct),
                typeof(ContentCategory)
            );
            Console.WriteLine("✅ 数据库同步成功！");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 同步失败: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"🔍 内部详情: {ex.InnerException.Message}");
            }
        }
    }
}
