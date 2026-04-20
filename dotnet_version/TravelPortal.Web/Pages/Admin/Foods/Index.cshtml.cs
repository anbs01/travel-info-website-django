using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Foods
{
    public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public IndexModel(ISqlSugarClient db)
        {
            _db = db;
        }

        public List<Models.Food> FoodList { get; set; } = new();
        public SelectList PlaceList { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PlaceId { get; set; }

        public void OnGet()
        {
            var places = _db.Queryable<Place>().ToList();
            PlaceList = new SelectList(places, "Id", "Title");

            var query = _db.Queryable<Models.Food>();

            if (!string.IsNullOrEmpty(Keyword))
            {
                query = query.Where(f => f.Title.Contains(Keyword));
            }

            if (PlaceId.HasValue)
            {
                query = query.Where(f => f.PlaceId == PlaceId);
            }

            FoodList = query.OrderByDescending(f => f.IsSticky)
                            .OrderByDescending(f => f.CreatedAt)
                            .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                await _db.Deleteable<Models.Food>().In(ids).ExecuteCommandAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleStickyAsync(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                var list = await _db.Queryable<Models.Food>().In(ids).ToListAsync();
                foreach (var item in list)
                {
                    item.IsSticky = !item.IsSticky;
                    await _db.Updateable(item).UpdateColumns(f => f.IsSticky).ExecuteCommandAsync();
                }
            }
            return RedirectToPage();
        }
    }
}
