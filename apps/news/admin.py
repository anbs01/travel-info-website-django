from django.contrib import admin
from .models import News


@admin.register(News)
class NewsAdmin(admin.ModelAdmin):
    list_display = ('title', 'slug', 'level_tag', 'source', 'author', 'is_sticky', 'is_home', 'is_hidden', 'created_at')
    list_filter = ('is_sticky', 'is_home', 'is_hidden')
    search_fields = ('title', 'summary', 'feature_tags')
    readonly_fields = ('slug', 'created_at', 'updated_at')  # slug 自动生成
    
    class Media:
        css = {
            'all': ('core/css/custom_admin.css',)
        }

    fieldsets = (
        ('基础信息', {
            'fields': ('title', 'full_title', 'slug')
        }),
        ('灵魂展示字段', {
            # level_tag: 资讯分类 (如：官方动态,行业新闻)
            # feature_tags: 横向彩色话题标签
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
