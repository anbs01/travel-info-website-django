using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.HotWords
{
    public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public IndexModel(ISqlSugarClient db)
        {
            _db = db;
        }

        public List<HotWord> HotWords { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; } = "Home";

        [BindProperty]
        public HotWord NewHotWord { get; set; } = new();

        public async Task OnGetAsync()
        {
            var query = _db.Queryable<HotWord>();

            // 根据 Tab 类型筛选
            query = Type switch
            {
                "Home" => query.Where(h => h.ShowInHome),
                "Place" => query.Where(h => h.ShowInPlace),
                "Scenic" => query.Where(h => h.ShowInScenic),
                "Travelogue" => query.Where(h => h.ShowInTravelogue),
                "Guide" => query.Where(h => h.ShowInGuide),
                "Specialty" => query.Where(h => h.ShowInSpecialty),
                "Cuisine" => query.Where(h => h.ShowInCuisine),
                "Creative" => query.Where(h => h.ShowInCreative),
                "News" => query.Where(h => h.ShowInNews),
                _ => query.Where(h => h.ShowInHome)
            };

            HotWords = await query.OrderBy(h => h.SortOrder).OrderByDescending(h => h.CreatedAt).ToListAsync();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            await _db.Insertable(NewHotWord).ExecuteCommandAsync();
            return RedirectToPage(new { type = Type });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _db.Deleteable<HotWord>().In(id).ExecuteCommandAsync();
            return RedirectToPage(new { type = Type });
        }

        public async Task<IActionResult> OnPostToggleHiddenAsync(int id)
        {
            var item = await _db.Queryable<HotWord>().InSingleAsync(id);
            if (item != null)
            {
                item.IsHidden = !item.IsHidden;
                await _db.Updateable(item).ExecuteCommandAsync();
            }
            return RedirectToPage(new { type = Type });
        }
    }
}
