from django.contrib import admin
from .models import Region, Place, ScenicSpot, Traffic


@admin.register(Region)
class RegionAdmin(admin.ModelAdmin):
    list_display = ('name', 'english_name', 'is_active', 'sort_order')
    list_editable = ('is_active', 'sort_order')
    search_fields = ('name', 'english_name')


@admin.register(Place)
class PlaceAdmin(admin.ModelAdmin):
    list_display = ('title', 'alias', 'region', 'english_code', 'level_tag', 'is_sticky', 'is_home', 'is_hidden')
    list_filter = ('region', 'is_sticky', 'is_home', 'is_hidden')
    search_fields = ('title', 'alias', 'english_code')
    readonly_fields = ('created_at', 'updated_at')
    fieldsets = (
        ('基础信息', {
            'fields': ('title', 'full_title', 'alias', 'english_code', 'region', 'parent')
        }),
        ('灵魂展示字段', {
            'fields': ('level_tag', 'feature_tags', 'best_time', 'summary')
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


@admin.register(ScenicSpot)
class ScenicSpotAdmin(admin.ModelAdmin):
    list_display = ('title', 'place', 'level_tag', 'is_sticky', 'is_home', 'is_hidden')
    list_filter = ('place', 'is_sticky', 'is_home', 'is_hidden')
    search_fields = ('title', 'address')
    readonly_fields = ('created_at', 'updated_at')
    fieldsets = (
        ('基础信息', {
            'fields': ('title', 'full_title', 'english_code', 'place', 'address', 'opening_hours', 'ticket_info')
        }),
        ('灵魂展示字段', {
            'fields': ('level_tag', 'feature_tags', 'summary')
        }),
        ('媒体与水印', {
            'fields': ('image', 'image_show_watermark', 'watermark_pos')
        }),
        ('运营控制', {
            'fields': ('is_sticky', 'is_home', 'is_hidden', 'redirect_url', 'views')
        }),
        ('系统字段', {
            'classes': ('collapse',),
            'fields': ('created_at', 'updated_at')
        }),
    )


@admin.register(Traffic)
class TrafficAdmin(admin.ModelAdmin):
    list_display = ('title', 'place', 'traffic_type', 'duration', 'is_sticky', 'is_hidden')
    list_filter = ('traffic_type', 'is_sticky', 'is_hidden')
    search_fields = ('title',)
    readonly_fields = ('created_at', 'updated_at')
