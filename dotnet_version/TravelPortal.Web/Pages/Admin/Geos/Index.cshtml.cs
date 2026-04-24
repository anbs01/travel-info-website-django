using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Geos;

public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;

    public IndexModel(ISqlSugarClient db)
    {
        _db = db;
    }

    public PaginatedList<Geo> GeoList { get; set; } = null!;

    [BindProperty(SupportsGet = true)]
    public int? ParentId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? Level { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Nature { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageIndex { get; set; } = 1;

    public Geo? ParentGeo { get; set; }

    public void OnGet()
    {
        var query = _db.Queryable<Geo>();

        if (ParentId.HasValue && ParentId > 0)
        {
            query = query.Where(it => it.ParentId == ParentId);
            ParentGeo = _db.Queryable<Geo>().InSingle(ParentId.Value);
        }
        else if (Level.HasValue)
        {
            query = query.Where(it => it.Level == Level);
        }

        if (!string.IsNullOrEmpty(Nature))
            query = query.Where(it => it.Nature == Nature);

        int total = 0;
        var items = query.OrderBy(it => it.Level).OrderBy(it => it.SortOrder).OrderBy(it => it.Id)
                         .ToPageList(PageIndex, 10, ref total);

        GeoList = new PaginatedList<Geo>(items, total, PageIndex, 10);
    }

    public IActionResult OnPostDelete(int id)
    {
        // 检查是否有下级
        if (_db.Queryable<Geo>().Any(it => it.ParentId == id))
        {
            TempData["Error"] = "该节点下有子节点，无法删除！";
            return RedirectToPage();
        }

        _db.Deleteable<Geo>().In(id).ExecuteCommand();
        return RedirectToPage();
    }
}
