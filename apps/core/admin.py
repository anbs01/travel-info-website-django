from django.contrib import admin
from .models import SiteInfo, HomeTextRecommend, SearchKeyword


@admin.register(SiteInfo)
class SiteInfoAdmin(admin.ModelAdmin):
    """网站基本信息 - 单例，禁止新增多条"""
    fieldsets = (
        ('常规页面', {'fields': ('about', 'service', 'contact', 'cooperation')}),
        ('版权信息', {'fields': ('copyright', 'copyright_home')}),
    )

    def has_add_permission(self, request):
        # 保证全局只有一条配置记录
        return not SiteInfo.objects.exists()


@admin.register(HomeTextRecommend)
class HomeTextRecommendAdmin(admin.ModelAdmin):
    list_display = ('title', 'url', 'is_active', 'sort_order')
    list_editable = ('is_active', 'sort_order')
    search_fields = ('title',)


@admin.register(SearchKeyword)
class SearchKeywordAdmin(admin.ModelAdmin):
    list_display = ('keyword', 'is_active', 'sort_order')
    list_editable = ('is_active', 'sort_order')
    search_fields = ('keyword',)
