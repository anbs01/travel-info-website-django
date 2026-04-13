from django.contrib import admin
from .models import Travelogue


@admin.register(Travelogue)
class TravelogueAdmin(admin.ModelAdmin):
    list_display = ('title', 'slug', 'level_tag', 'place', 'source', 'author', 'is_sticky', 'is_home', 'is_hidden', 'created_at')
    list_filter = ('place', 'is_sticky', 'is_home', 'is_hidden')
    search_fields = ('title', 'summary', 'feature_tags', 'content')
    readonly_fields = ('slug', 'created_at', 'updated_at')  # slug 自动生成
    fieldsets = (
        ('基础信息', {
            'fields': ('title', 'full_title', 'slug', 'place')
        }),
        ('灵魂展示字段', {
            # level_tag: 游记类型 (如：亲子游,自驾游,摄影游记)
            # feature_tags: 亮点标签云
            'fields': ('level_tag', 'feature_tags', 'summary')
        }),
        ('富文本正文', {
            'fields': ('content',)
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
