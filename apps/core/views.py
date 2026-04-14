from django.shortcuts import render
from django.views.generic import TemplateView
from core.models import HomeTextRecommend, SearchKeyword
from travelogue.models import Travelogue

from django.db.models import F

class IndexView(TemplateView):
    template_name = 'pages/home.html'

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
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

class DetailSampleView(TemplateView):
    template_name = 'pages/detail_sample.html'
