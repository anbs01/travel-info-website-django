from django.views.generic import DetailView, ListView
from django.db.models import Q
from .models import Travelogue
from places.models import Region
from core.views import ViewCountMixin, DetailRedirectMixin

class TravelogueListView(ListView):
    model = Travelogue
    template_name = 'pages/travelogue_list.html'
    context_object_name = 'travelogues'
    paginate_by = 10

    def get_queryset(self):
        queryset = super().get_queryset().filter(is_hidden=False)
        
        # 1. 地区过滤
        region_id = self.request.GET.get('region')
        if region_id:
            queryset = queryset.filter(place__region_id=region_id)
            
        # 2. 关键字搜索
        q = self.request.GET.get('q')
        if q:
            queryset = queryset.filter(
                Q(title__icontains=q) | 
                Q(summary__icontains=q) |
                Q(feature_tags__icontains=q)
            )

        # 3. 标签过滤 (例如：攻略, 路线, 玩法)
        tag = self.request.GET.get('tag')
        if tag:
            queryset = queryset.filter(feature_tags__icontains=tag)
            
        return queryset

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context['all_regions'] = Region.objects.filter(is_active=True)
        # 设计稿中常用的分类标签
        context['category_tags'] = ['攻略', '路线', '玩法', '目的地', '交通', '住宿']
        return context


class TravelogueDetailView(DetailRedirectMixin, ViewCountMixin, DetailView):
    model = Travelogue
    template_name = 'pages/travelogue_detail.html'
    context_object_name = 'travelogue'
    slug_field = 'slug'
    slug_url_kwarg = 'slug'

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        # 获取相关推荐
        if self.object.place:
            context['related_travelogues'] = Travelogue.objects.filter(
                place=self.object.place
            ).exclude(id=self.object.id)[:5]
        return context
