using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.Admin.Recommendations
{
    public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;
        private readonly IUploadService _uploadService;

        public IndexModel(ISqlSugarClient db, IUploadService uploadService)
        {
            _db = db;
            _uploadService = uploadService;
        }

        public PaginatedList<Recommendation> Recommendations { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        [BindProperty]
        public Recommendation RecommendationForm { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task OnGetAsync()
        {
            int pageSize = 10;
            var query = _db.Queryable<Recommendation>();

            // 筛选逻辑
            if (StatusFilter == "Showing")
            {
                query = query.Where(r => r.EndDate == null || r.EndDate > DateTime.Now);
            }
            else if (StatusFilter == "Expired")
            {
                query = query.Where(r => r.EndDate != null && r.EndDate <= DateTime.Now);
            }

            RefAsync<int> totalCount = 0;
            // 排序逻辑：置顶在前，其次按添加日期倒序
            var list = await query.OrderByDescending(r => r.IsPinned)
                                 .OrderByDescending(r => r.CreatedAt)
                                 .ToPageListAsync(PageIndex, pageSize, totalCount);

            Recommendations = new PaginatedList<Recommendation>(list, totalCount, PageIndex, pageSize);
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            // 使用公共服务处理图片上传
            if (ImageFile != null)
            {
                RecommendationForm.ImageUrl = await _uploadService.UploadFileAsync(ImageFile, "recommendations");
            }

            if (RecommendationForm.Id > 0)
            {
                // 更新时忽略不需要修改的字段
                var updateable = _db.Updateable(RecommendationForm)
                                   .IgnoreColumns(r => new { r.CreatedAt, r.ClickCount, r.SortOrder });
                
                // 如果没有上传新图片，则保留原图片地址
                if (ImageFile == null)
                {
                    updateable = updateable.IgnoreColumns(r => r.ImageUrl);
                }

                await updateable.ExecuteCommandAsync();
            }
            else
            {
                RecommendationForm.CreatedAt = DateTime.Now;
                await _db.Insertable(RecommendationForm).ExecuteCommandAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _db.Deleteable<Recommendation>().In(id).ExecuteCommandAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostTogglePinAsync(int id, bool isPinned)
        {
            await _db.Updateable<Recommendation>()
                     .SetColumns(r => r.IsPinned == isPinned)
                     .Where(r => r.Id == id)
                     .ExecuteCommandAsync();
            return RedirectToPage();
        }
    }
}
