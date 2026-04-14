from django.views.generic import ListView, DetailView
from django.db.models import Q
from .models import Food
from places.models import Region
from core.views import ViewCountMixin

class FoodListView(ListView):
    # ... (原有代码保持不变)
    model = Food
    template_name = 'pages/food_list.html'
    context_object_name = 'foods'
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
        # 美食栏目常用标签
        context['category_tags'] = ['特色菜', '小吃', '伴手礼', '老字号', '非遗美食']
        return context

class FoodDetailView(ViewCountMixin, DetailView):
    model = Food
    template_name = 'pages/food_detail.html'
    context_object_name = 'food'
    slug_field = 'english_code'
    slug_url_kwarg = 'slug'

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        # 获取相关推荐：同城镇的其他美食
        context['related_foods'] = Food.objects.filter(
            place=self.object.place
        ).exclude(id=self.object.id)[:4]
        return context
