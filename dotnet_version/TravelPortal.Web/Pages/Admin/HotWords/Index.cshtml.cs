using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.HotWords
{
    public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public IndexModel(ISqlSugarClient db) => _db = db;

        public PaginatedList<HotWord> HotWords { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        // Tab 当前模块，对应 HotWord.MOD_xxx 常量
        [BindProperty(SupportsGet = true)]
        public string Module { get; set; } = HotWord.MOD_HOME;

        [BindProperty]
        public HotWord NewHotWord { get; set; } = new();

        // Tab 定义：(模块值, 显示名)
        public static readonly (string Id, string Name)[] Tabs = new[]
        {
            (HotWord.MOD_HOME,       "首页搜索词"),
            (HotWord.MOD_PLACE,      "城乡搜索词"),
            (HotWord.MOD_SCENIC,     "打卡地类别"),
            (HotWord.MOD_TRAVELOGUE, "纪行类别"),
            (HotWord.MOD_GUIDE,      "攻略类别"),
            (HotWord.MOD_SPECIALTY,  "特产类别"),
            (HotWord.MOD_CUISINE,    "美食菜系"),
            (HotWord.MOD_CREATIVE,   "文创类别"),
            (HotWord.MOD_NEWS,       "资讯类别"),
        };

        public async Task OnGetAsync()
        {
            const int pageSize = 15;
            RefAsync<int> totalCount = 0;

            var list = await _db.Queryable<HotWord>()
                .Where(h => h.Module == Module)
                .OrderBy(h => h.SortOrder)
                .OrderByDescending(h => h.CreatedAt)
                .ToPageListAsync(PageIndex, pageSize, totalCount);

            HotWords = new PaginatedList<HotWord>(list, totalCount, PageIndex, pageSize);
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            // 强制 Module 与当前 Tab 一致
            NewHotWord.Module = Module;
            await _db.Insertable(NewHotWord).ExecuteCommandAsync();
            return RedirectToPage(new { module = Module });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _db.Deleteable<HotWord>().In(id).ExecuteCommandAsync();
            return RedirectToPage(new { module = Module });
        }

        public async Task<IActionResult> OnPostToggleHiddenAsync(int id)
        {
            var item = await _db.Queryable<HotWord>().InSingleAsync(id);
            if (item != null)
            {
                item.IsHidden = !item.IsHidden;
                await _db.Updateable(item).UpdateColumns(h => new { h.IsHidden }).ExecuteCommandAsync();
            }
            return RedirectToPage(new { module = Module });
        }
    }
}
