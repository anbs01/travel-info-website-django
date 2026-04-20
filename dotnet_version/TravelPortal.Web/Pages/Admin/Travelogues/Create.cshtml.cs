using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Travelogues
{
    public class CreateModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public CreateModel(ISqlSugarClient db)
        {
            _db = db;
        }

        [BindProperty]
        public Travelogue Travelogue { get; set; } = new();

        public SelectList PlaceList { get; set; } = default!;

        // 动态加载的热词列表
        public List<HotWord> TravelogueCategories { get; set; } = new();
        public List<HotWord> GuideCategories { get; set; } = new();

        [BindProperty]
        public string[] SelectedTags { get; set; } = Array.Empty<string>();

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
            Travelogue.Category = "travelogue";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            // 处理多选标签
            if (SelectedTags != null && SelectedTags.Length > 0)
            {
                Travelogue.Tags = string.Join(",", SelectedTags);
            }

            Travelogue.Slug = DateTime.Now.ToString("yyyyMMddHHmm");
            await _db.Insertable(Travelogue).ExecuteCommandAsync();

            TempData["SuccessMessage"] = "作品已成功发布";
            return RedirectToPage("Index");
        }

        private async Task LoadDataAsync()
        {
            // 加载城镇
            var places = await _db.Queryable<Place>().OrderBy(p => p.Title).ToListAsync();
            PlaceList = new SelectList(places, "Id", "Title");

            // 加载热词分类
            TravelogueCategories = await _db.Queryable<HotWord>()
                .Where(h => h.ShowInTravelogue && !h.IsHidden)
                .OrderBy(h => h.SortOrder).ToListAsync();

            GuideCategories = await _db.Queryable<HotWord>()
                .Where(h => h.ShowInGuide && !h.IsHidden)
                .OrderBy(h => h.SortOrder).ToListAsync();
        }
    }
}
