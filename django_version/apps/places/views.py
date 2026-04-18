from django.views.generic import ListView, DetailView
from django.db.models import Q
from .models import Place, Region, ScenicSpot, Traffic
from foods.models import Food
from goods.models import Good
from travelogue.models import Travelogue
from core.views import ViewCountMixin, DetailRedirectMixin

class PlaceListView(ListView):
    # ... (原有逻辑保持不变)
    model = Place
    template_name = 'pages/place_list.html'
    context_object_name = 'places'
    paginate_by = 12

    def get_queryset(self):
        queryset = super().get_queryset().filter(is_hidden=False)
        
        # 1. 关键字过滤
        q = self.request.GET.get('q')
        if q:
            queryset = queryset.filter(
                Q(title__icontains=q) | 
                Q(full_title__icontains=q) |
                Q(alias__icontains=q)
            )
            
        # 2. 地区过滤
        region_id = self.request.GET.get('region')
        if region_id:
            queryset = queryset.filter(region_id=region_id)

        # 3. 排序策略 (需求第10条): 置顶优先(is_sticky)，且置顶内按置顶时间倒序，普通按创建时间倒序
        return queryset.order_by('-is_sticky', '-sticky_at', '-created_at')

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context['all_regions'] = Region.objects.filter(is_active=True)
        return context

class PlaceDetailView(DetailRedirectMixin, ViewCountMixin, DetailView):
    model = Place
    template_name = 'pages/place_detail.html'
    context_object_name = 'place'
    slug_field = 'english_code'
    slug_url_kwarg = 'slug'

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        # 聚合各应用数据
        place = self.object
        # 1. 打卡地
        context['scenic_spots'] = ScenicSpot.objects.filter(place=place, is_hidden=False)[:10]
        # 2. 特产美食
        context['foods'] = Food.objects.filter(place=place, is_hidden=False)[:10]
        # 3. 文创好物
        context['goods'] = Good.objects.filter(place=place, is_hidden=False)[:10]
        # 4. 交通出行
        context['traffic_info'] = Traffic.objects.filter(place=place, is_hidden=False)
        # 5. 关联攻略
        context['travelogues'] = Travelogue.objects.filter(place=place, is_hidden=False)[:10]
        
        return context
