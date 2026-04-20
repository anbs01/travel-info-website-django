using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public SelectList PlaceList { get; set; } = null!;

        public void OnGet()
        {
            var places = _db.Queryable<Place>().ToList();
            PlaceList = new SelectList(places, "Id", "Title");
            News.PublishDate = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var places = _db.Queryable<Place>().ToList();
                PlaceList = new SelectList(places, "Id", "Title");
                return Page();
            }

            News.CreatedAt = DateTime.Now;
            News.UpdatedAt = DateTime.Now;
            
            await _db.Insertable(News).ExecuteCommandAsync();
            
            return RedirectToPage("Index");
        }
    }
}
