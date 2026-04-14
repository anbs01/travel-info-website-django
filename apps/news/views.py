from django.views.generic import ListView, DetailView
from django.db.models import Q
from .models import News
from places.models import Region
from core.views import ViewCountMixin, DetailRedirectMixin

class NewsListView(ListView):
    # ... (原有代码保持不变)
    model = News
    template_name = 'pages/news_list.html'
    context_object_name = 'news_list'
    paginate_by = 15

    def get_queryset(self):
        queryset = super().get_queryset().filter(is_hidden=False)
        region_id = self.request.GET.get('region')
        if region_id:
            queryset = queryset.filter(place__region_id=region_id)
        
        q = self.request.GET.get('q')
        if q:
            queryset = queryset.filter(
                Q(title__icontains=q) | 
                Q(summary__icontains=q)
            )

        tag = self.request.GET.get('tag')
        if tag:
            queryset = queryset.filter(feature_tags__icontains=tag)
            
        return queryset

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context['all_regions'] = Region.objects.filter(is_active=True)
        # 资讯栏目常用标签
        context['category_tags'] = ['官方动态', '行业新闻', '活动预告', '媒体报道']
        return context

class NewsDetailView(DetailRedirectMixin, ViewCountMixin, DetailView):
    model = News
    template_name = 'pages/news_detail.html'
    context_object_name = 'news'
    slug_field = 'slug'
    slug_url_kwarg = 'slug'

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        # 获取相关资讯推荐 (同目的地下除当前篇以外的资讯)
        if self.object.place:
            context['related_news'] = News.objects.filter(
                place=self.object.place
            ).exclude(id=self.object.id)[:8]
        else:
            context['related_news'] = News.objects.filter(
                is_hidden=False
            ).exclude(id=self.object.id).order_by('-created_at')[:8]
        return context
