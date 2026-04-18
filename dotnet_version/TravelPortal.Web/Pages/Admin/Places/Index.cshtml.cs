using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Places;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public IndexModel(ISqlSugarClient db)
    {
        _db = db;
    }

    public List<Place> PlaceList { get; set; } = new();

    public void OnGet()
    {
        // 读取所有目的地，按排序权重降序
        PlaceList = _db.Queryable<Place>().OrderBy(it => it.SortOrder, OrderByType.Desc).ToList();
    }
}
