from django.shortcuts import render, redirect, reverse
from django.views import View
from django.db.models import Q, F
from django.views.generic import TemplateView
from django.http import JsonResponse, HttpResponseRedirect
from core.models import HomeTextRecommend, SearchKeyword, SiteInfo, Feedback
from travelogue.models import Travelogue
from news.models import News
from foods.models import Food
from goods.models import Good
from places.models import Place

class IndexView(TemplateView):
    template_name = 'pages/home.html'

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context['is_home'] = True
        # 获取首页焦点图推荐
        context['featured_travelogues'] = Travelogue.objects.filter(is_home=True, is_hidden=False)[:3]
        # 获取首页文字推荐
        context['home_recommends'] = HomeTextRecommend.objects.filter(is_active=True)[:8]
        # 获取热门搜索词
        context['search_keywords'] = SearchKeyword.objects.filter(is_active=True)[:5]
        return context

class ViewCountMixin:
    """详情页阅读量自增 Mixin"""
    def get_object(self, queryset=None):
        obj = super().get_object(queryset)
        # 仅在模型有 views 字段时自增
        if obj and hasattr(obj, 'views'):
            # 使用 F 表达式防止并发冲突
            obj.views = F('views') + 1
            obj.save(update_fields=['views'])
            obj.refresh_from_db()
        return obj

class DetailRedirectMixin:
    """详情页重定向 Mixin - 检查 BaseContent.redirect_url"""
    def get(self, request, *args, **kwargs):
        # 预先获取对象以检查重定向标识，避免在 super().get 之前多次触发复杂逻辑
        obj = self.get_object()
        if obj and hasattr(obj, 'redirect_url') and obj.redirect_url:
            return HttpResponseRedirect(obj.redirect_url)
        
        # 预存对象，减少 super().get 时的重复查询开销（Django DetailView 会重复使用 self.object）
        self.object = obj
        return super().get(request, *args, **kwargs)

class DetailSampleView(TemplateView):
    template_name = 'pages/detail_sample.html'

class GlobalSearchView(View):
    """全局搜索中转与聚合视图"""
    def get(self, request, *args, **kwargs):
        q = request.GET.get('q', '')
        search_type = request.GET.get('type', '')

        # 1. 如果指定了分类，直接分发跳转
        if search_type:
            app_map = {
                'travelogue': 'travelogue:list',
                'places': 'places:list',
                'foods': 'foods:list',
                'goods': 'goods:list',
                'news': 'news:list',
            }
            if search_type in app_map:
                return redirect(reverse(app_map[search_type]) + f"?q={q}")

        # 2. 如果是公共聚合搜索（或未指定分类）
        results = {
            'news': News.objects.filter(Q(title__icontains=q) | Q(summary__icontains=q), is_hidden=False)[:5],
            'travelogues': Travelogue.objects.filter(Q(title__icontains=q) | Q(summary__icontains=q), is_hidden=False)[:5],
            'places': Place.objects.filter(Q(title__icontains=q) | Q(alias__icontains=q), is_hidden=False)[:5],
            'foods': Food.objects.filter(Q(title__icontains=q) | Q(summary__icontains=q), is_hidden=False)[:3],
            'goods': Good.objects.filter(Q(title__icontains=q) | Q(summary__icontains=q), is_hidden=False)[:3],
        }
        
        # 计算总结果数
        total_count = sum(len(v) for v in results.values())

        return render(request, 'pages/search_results.html', {
            'q': q,
            'results': results,
            'total_count': total_count
        })

class InfoPageView(TemplateView):
    """
    通用静态专题页视图
    根据 URL 别名匹配 SiteInfo 中的富文本字段
    """
    template_name = 'pages/info_page.html'

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        page_type = self.kwargs.get('page_type', 'about')
        site_info = SiteInfo.objects.first()
        
        # 映射字段名与标题
        mapping = {
            'about': ('关于我们', 'about'),
            'service': ('服务协议', 'service'),
            'cooperation': ('合作事宜', 'cooperation'),
            'contact': ('联系我们', 'contact'),
        }
        
        title, field_name = mapping.get(page_type, mapping['about'])
        context['page_title'] = title
        context['page_type'] = page_type
        context['content'] = getattr(site_info, field_name, '') if site_info else "内容正在补充中..."
        return context

class FeedbackView(View):
    """处理用户反馈提交 (AJAX)"""
    def post(self, request, *args, **kwargs):
        title = request.POST.get('title', '意见建议')
        content = request.POST.get('content')
        name = request.POST.get('name', '')
        contact = request.POST.get('contact', '')
        source_url = request.POST.get('source_url', request.META.get('HTTP_REFERER', ''))

        if not content:
            return JsonResponse({'status': 'error', 'message': '请填写反馈内容'}, status=400)

        Feedback.objects.create(
            title=title,
            content=content,
            name=name,
            contact=contact,
            source_url=source_url
        )
        
        return JsonResponse({'status': 'success', 'message': '感谢您的反馈，我们将尽快处理！'})
