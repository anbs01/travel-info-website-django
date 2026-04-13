from django.db import models
from core.models import BaseContent
import time

class News(BaseContent):
    """资讯·动态 - 灵魂基准版"""
    slug = models.SlugField('URL标识', max_length=100, unique=True, blank=True)
    
    # level_tag 用于存储“资讯分类” (如：官方动态, 行业新闻)
    # feature_tags 用于支持横向彩色标签

    class Meta:
        verbose_name = '资讯·动态'
        verbose_name_plural = verbose_name

    def save(self, *args, **kwargs):
        if not self.slug:
            self.slug = f"news-{int(time.time())}"
        super().save(*args, **kwargs)
