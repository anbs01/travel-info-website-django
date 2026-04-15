from django.shortcuts import get_object_or_404
from django.views.generic import ListView, DetailView
from django.db.models import Q
from .models import Good
from places.models import Region
from core.views import ViewCountMixin, DetailRedirectMixin

class GoodListView(ListView):
    # ... (原有代码保持不变)
    model = Good
    template_name = 'pages/good_list.html'
    context_object_name = 'goods'
    paginate_by = 10

    def get_queryset(self):
        queryset = super().get_queryset().filter(is_hidden=False)
        region_id = self.request.GET.get('region')
        if region_id:
            queryset = queryset.filter(place__region_id=region_id)
        
        q = self.request.GET.get('q')
        if q:
            queryset = queryset.filter(
                Q(title__icontains=q) | 
                Q(summary__icontains=q) |
                Q(feature_tags__icontains=q)
            )

        tag = self.request.GET.get('tag')
        if tag:
            queryset = queryset.filter(feature_tags__icontains=tag)
            
        return queryset

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context['all_regions'] = Region.objects.filter(is_active=True)
        # 好物栏目常用标签
        context['category_tags'] = ['工艺品', '纪念品', '生活用品', '文创周边', '非遗']
        return context

class GoodDetailView(DetailRedirectMixin, ViewCountMixin, DetailView):
    model = Good
    template_name = 'pages/good_detail.html'
    context_object_name = 'good'
    slug_field = 'english_code'
    slug_url_kwarg = 'slug'

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        # 获取相关推荐：同城镇的其他好物
        context['related_goods'] = Good.objects.filter(
            place=self.object.place
        ).exclude(id=self.object.id)[:4]
        return context
