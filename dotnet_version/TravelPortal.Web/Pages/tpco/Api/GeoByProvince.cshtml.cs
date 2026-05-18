using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Utils;

namespace TravelPortal.Web.Pages.tpco.Api;

public class GeoByProvinceModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    public GeoByProvinceModel(ISqlSugarClient db) => _db = db;

    public IActionResult OnGet(int? parentId, string? search)
    {
        var query = _db.Queryable<Geo>();

        if (!string.IsNullOrEmpty(search))
        {
            // 搜索模式：使用 LeftJoin 联表获取父级名称，确保无父级的顶级项或断层项也能被搜索到
            var allGeos = _db.Queryable<Geo>()
                .LeftJoin<Geo>((g, p) => g.ParentId == p.Id)
                .Select((g, p) => new { 
                    g.Id, 
                    g.Title, 
                    g.Level, 
                    g.ParentId,
                    g.Slug,
                    g.EnglishName,
                    ParentTitle = p.Title 
                })
                .ToList();

            var searchResults = allGeos
                .Where(g => {
                    // 1. 如果包含汉字，使用中文名称模糊匹配
                    if (search.Any(c => c >= 0x4e00 && c <= 0x9fbb))
                    {
                        return g.Title.Contains(search, StringComparison.OrdinalIgnoreCase);
                    }

                    // 2. 如果是纯英文/拼音输入
                    var initials = PinyinHelper.GetInitials(g.Title);
                    
                    // 始终支持拼音首字母匹配（如 sh 匹配 上海 SH，sd 匹配 山东 SD）
                    if (initials.Contains(search, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    // 支持第一字的全拼匹配（如 si 匹配 四川 sichuan，su 匹配 苏州 suzhou）
                    var firstSyllable = GetFirstSyllable(g.Slug, initials);
                    if (firstSyllable.Equals(search, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    // 如果输入长度大于或等于第一字全拼长度，支持完整全拼或英文名称前缀匹配
                    if (search.Length >= firstSyllable.Length)
                    {
                        return (g.Slug != null && g.Slug.StartsWith(search, StringComparison.OrdinalIgnoreCase)) ||
                               (g.EnglishName != null && g.EnglishName.StartsWith(search, StringComparison.OrdinalIgnoreCase));
                    }

                    return false;
                })
                .Select(g => new { 
                    g.Id, 
                    g.Title, 
                    g.Level, 
                    g.ParentId,
                    g.ParentTitle,
                    FirstLetter = PinyinHelper.GetFirstLetter(g.Title)
                })
                .ToList();

            return new JsonResult(searchResults);
        }

        var items = query.Where(g => g.ParentId == parentId)
            .OrderBy(g => g.Title)
            .ToList()
            .Select(g => new { 
                g.Id, 
                g.Title, 
                g.Level, 
                g.ParentId,
                FirstLetter = PinyinHelper.GetFirstLetter(g.Title)
            })
            .ToList();

        return new JsonResult(items);
    }

    /// <summary>
    /// 根据拼音别名和首字母自动提取第一个汉字字符的完整拼音（如从 sichuan 和 SC 解析出 si）
    /// </summary>
    private static string GetFirstSyllable(string? slug, string initials)
    {
        if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(initials) || initials.Length < 2)
        {
            return slug ?? string.Empty;
        }

        char secondInitial = char.ToLower(initials[1]);
        int startSearchIndex = 1;
        if (slug.Length > 1 && (slug.StartsWith("zh") || slug.StartsWith("ch") || slug.StartsWith("sh")))
        {
            startSearchIndex = 2;
        }

        int secondInitialIndex = slug.IndexOf(secondInitial, startSearchIndex);
        if (secondInitialIndex > 0)
        {
            return slug.Substring(0, secondInitialIndex);
        }

        return slug;
    }
}
