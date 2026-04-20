using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.News
{
    public class CreateModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public CreateModel(ISqlSugarClient db)
        {
            _db = db;
        }

        [BindProperty]
        public Models.News News { get; set; } = new();

        public void OnGet()
        {
            News.PublishDate = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            News.CreatedAt = DateTime.Now;
            News.UpdatedAt = DateTime.Now;
            
            await _db.Insertable(News).ExecuteCommandAsync();
            
            return RedirectToPage("Index");
        }
    }
}
