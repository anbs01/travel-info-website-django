from django.contrib import admin
from .models import Food


@admin.register(Food)
class FoodAdmin(admin.ModelAdmin):
    list_display = ('title', 'place', 'level_tag', 'is_sticky', 'is_home', 'is_hidden', 'created_at')
    list_filter = ('place', 'is_sticky', 'is_home', 'is_hidden')
    search_fields = ('title', 'summary', 'feature_tags')
    readonly_fields = ('created_at', 'updated_at')
    
    class Media:
        css = {
            'all': ('core/css/custom_admin.css',)
        }

    fieldsets = (
        ('基础信息', {
            'fields': ('title', 'full_title', 'english_code', 'place')
        }),
        ('灵魂展示字段', {
            # level_tag: 菜系或非遗级别标签 (右上角彩色标签)
            # feature_tags: 特色标签云 (如：鲜甜,清淡,古法工艺)
            'fields': ('level_tag', 'feature_tags', 'summary')
        }),
        ('正文详情', {
            'fields': ('content',),
        }),
        ('媒体与水印', {
            'fields': ('image', 'image_show_watermark', 'watermark_pos')
        }),
        ('版权与来源', {
            'fields': ('source', 'author', 'publish_date')
        }),
        ('运营控制', {
            'fields': ('is_sticky', 'is_home', 'is_hidden', 'redirect_url', 'views')
        }),
        ('系统字段', {
            'classes': ('collapse',),
            'fields': ('created_at', 'updated_at')
        }),
    )
