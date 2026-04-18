from django.db import models
from django.utils import timezone
from core.models import BaseContent

class News(BaseContent):
    """资讯·动态 - 灵魂基准版"""
    place = models.ForeignKey('places.Place', on_delete=models.SET_NULL, null=True, blank=True, verbose_name='关联目的地')
    slug = models.SlugField('URL标识', max_length=20, unique=True, blank=True, help_text='系统自动生成：YYYYMMDDHHmm格式')
    
    # level_tag 用于存储“资讯分类” (如：官方动态, 行业新闻)
    # feature_tags 用于支持横向彩色标签

    class Meta:
        verbose_name = '资讯·动态'
        verbose_name_plural = verbose_name

    def save(self, *args, **kwargs):
        if not self.slug:
            # 优先使用 publish_date，若为空则使用当前时间
            ref_date = self.publish_date if self.publish_date else timezone.now()
            base_slug = ref_date.strftime('%Y%m%d%H%M')
            # 简单防重处理
            if News.objects.filter(slug=base_slug).exists():
                base_slug = ref_date.strftime('%Y%m%d%H%M%S')[-12:]
            self.slug = base_slug
        super().save(*args, **kwargs)
