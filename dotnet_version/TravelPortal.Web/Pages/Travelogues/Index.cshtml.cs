using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;

namespace TravelPortal.Web.Pages.Travelogues
{
    public class IndexModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISqlSugarClient _db;

        public IndexModel(ISqlSugarClient db)
        {
            _db = db;
        }

        public List<Travelogue> Travelogues { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        public void OnGet()
        {
            var query = _db.Queryable<Travelogue>()
                           .Where(t => !t.IsHidden);

            if (!string.IsNullOrEmpty(Category) && Category != "all")
            {
                query = query.Where(t => t.Category == Category);
            }

            Travelogues = query.OrderByDescending(t => t.IsSticky)
                               .OrderByDescending(t => t.PublishDate)
                               .ToList();
        }
    }
}
