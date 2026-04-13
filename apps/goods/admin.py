from django.contrib import admin
from .models import Good


@admin.register(Good)
class GoodAdmin(admin.ModelAdmin):
    list_display = ('title', 'place', 'level_tag', 'price', 'is_sticky', 'is_home', 'is_hidden', 'created_at')
    list_filter = ('place', 'is_sticky', 'is_home', 'is_hidden')
    search_fields = ('title', 'summary', 'feature_tags')
    readonly_fields = ('created_at', 'updated_at')
    fieldsets = (
        ('基础信息', {
            'fields': ('title', 'full_title', 'english_code', 'place', 'price')
        }),
        ('灵魂展示字段', {
            # level_tag: 非遗级别标签 (如：国家级非遗、省级非遗)
            # feature_tags: 产品特色标签云 (如：手工,限量,传承)
            'fields': ('level_tag', 'feature_tags', 'summary')
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
