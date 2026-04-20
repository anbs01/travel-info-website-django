using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Admin.Foods
{
    public class CreateModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public CreateModel(ISqlSugarClient db)
        {
            _db = db;
        }

        [BindProperty]
        public Models.Food Food { get; set; } = new();

        public SelectList PlaceList { get; set; } = null!;

        public void OnGet()
        {
            var places = _db.Queryable<Place>().ToList();
            PlaceList = new SelectList(places, "Id", "Title");
            Food.PublishDate = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var places = _db.Queryable<Place>().ToList();
                PlaceList = new SelectList(places, "Id", "Title");
                return Page();
            }

            Food.CreatedAt = DateTime.Now;
            Food.UpdatedAt = DateTime.Now;
            
            await _db.Insertable(Food).ExecuteCommandAsync();
            
            return RedirectToPage("Index");
        }
    }
}
