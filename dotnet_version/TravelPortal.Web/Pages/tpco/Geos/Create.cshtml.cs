using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SqlSugar;
using TravelPortal.Web.Models;
using TravelPortal.Web.Services;

namespace TravelPortal.Web.Pages.tpco.Geos;

public class CreateModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    private readonly ISqlSugarClient _db;
    private readonly IUploadService _uploadService;

    public CreateModel(ISqlSugarClient db, IUploadService uploadService)
    {
        _db = db;
        _uploadService = uploadService;
    }

    [BindProperty]
    public Geo Geo { get; set; } = new();

    [BindProperty]
    public IFormFile? MainImageFile { get; set; }

    public string PageTitle { get; set; } = "添加节点";
    public List<SelectListItem> ParentOptions { get; set; } = new();
    
    public Geo? ParentGeo { get; set; }

    public void OnGet(int? parentId, int? level, string? nature)
    {
        Geo.ParentId = parentId;
        Geo.Level = level ?? 1;
        Geo.Nature = nature ?? "Domestic";

        if (Geo.ParentId.HasValue && Geo.ParentId > 0)
        {
            ParentGeo = _db.Queryable<Geo>().InSingle(Geo.ParentId.Value);
        }

        UpdatePageTitle();
        LoadParents();
    }

    private void UpdatePageTitle()
    {
        if (Geo.Level == 1) PageTitle = "发布国家";
        else if (Geo.Level == 2) PageTitle = "发布省份";
        else if (Geo.Level >= 3) PageTitle = "发布城镇";
        if (Geo.Nature == "Overseas") PageTitle = "发布海外城镇";
    }

    private void LoadParents()
    {
        // 允许选择比当前层级更小的所有节点作为父级（支持跳级/断层录入）
        if (Geo.Level > 1)
        {
            var parents = _db.Queryable<Geo>()
                .Where(it => it.Level < Geo.Level)
                .OrderBy(it => it.Level)
                .OrderBy(it => it.SortOrder)
                .ToList();

            ParentOptions = parents.Select(it => new SelectListItem
            {
                Value = it.Id.ToString(),
                Text = $"[{it.Level}级] {it.Title}",
                Selected = it.Id == Geo.ParentId
            }).ToList();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // 后端强一致性安全校验：防行政级别冲突越级
        if (Geo.ParentId.HasValue && Geo.ParentId > 0)
        {
            ParentGeo = _db.Queryable<Geo>().InSingle(Geo.ParentId.Value);
            if (ParentGeo != null && Geo.Level <= ParentGeo.Level)
            {
                ModelState.AddModelError("Geo.Level", $"行政层级冲突！新建节点的层级({Geo.Level})必须低于父级节点“{ParentGeo.Title}”的层级({ParentGeo.Level})。");
            }
        }

        if (Geo.Level == 1)
        {
            if (string.IsNullOrEmpty(Geo.Slug))
            {
                Geo.Slug = TravelPortal.Web.Utils.PinyinHelper.GetInitials(Geo.Title).ToLower();
            }
        }

        // 城乡代码校验：只能是纯英文（无空格），且判断唯一性
        if (Geo.Level > 1)
        {
            if (string.IsNullOrWhiteSpace(Geo.Slug))
            {
                ModelState.AddModelError("Geo.Slug", "城乡代码不能为空。");
            }
            else
            {
                Geo.Slug = Geo.Slug.Trim().ToLower();
                if (!global::System.Text.RegularExpressions.Regex.IsMatch(Geo.Slug, @"^[a-zA-Z]+$"))
                {
                    ModelState.AddModelError("Geo.Slug", "城乡代码只能是纯英文字母（且不能有空格和特殊字符）。");
                }
                else
                {
                    bool isUnique = !_db.Queryable<Geo>().Any(it => it.Slug == Geo.Slug);
                    if (!isUnique)
                    {
                        ModelState.AddModelError("Geo.Slug", "该城乡代码已存在，请使用其他唯一的代码。");
                    }
                }
            }
        }

        // 后端强一致性安全校验：省份介绍必填
        if (Geo.Level == 2)
        {
            if (string.IsNullOrWhiteSpace(Geo.Content))
            {
                ModelState.AddModelError("Geo.Content", "省份介绍不能为空。");
            }
        }

        if (!ModelState.IsValid)
        {
            UpdatePageTitle();
            LoadParents();
            return Page();
        }

        if (MainImageFile != null)
        {
            Geo.MainImage = await _uploadService.UploadFileAsync(MainImageFile, "geos");
        }

        // 处理 AncestorPath
        if (Geo.ParentId.HasValue && Geo.ParentId > 0)
        {
            var parent = _db.Queryable<Geo>().InSingle(Geo.ParentId.Value);
            if (parent != null)
            {
                Geo.AncestorPath = string.IsNullOrEmpty(parent.AncestorPath) 
                    ? parent.Id.ToString() 
                    : $"{parent.AncestorPath}/{parent.Id}";
            }
        }

        Geo.CreatedAt = DateTime.Now;
        Geo.UpdatedAt = DateTime.Now;

        _db.Insertable(Geo).ExecuteCommand();
        return RedirectToPage("Index", new { parentId = Geo.ParentId, level = Geo.Level, nature = Geo.Nature });
    }

    public async Task<JsonResult> OnPostUploadMedia(IFormFile file)
    {
        if (file == null) return new JsonResult(new { error = "未选择文件" });
        var url = await _uploadService.UploadFileAsync(file, "editor/geos");
        return new JsonResult(new { location = url });
    }
}
