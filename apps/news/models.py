from django.db import models
from core.models import BaseContent

class News(BaseContent):
    """资讯新闻模型"""
    NEWS_CATEGORY_CHOICES = [
        ('hot', '热点'),
        ('notice', '公告'),
        ('activity', '活动'),
    ]

    category = models.CharField('资讯分类', max_length=20, choices=NEWS_CATEGORY_CHOICES, default='hot')
    author = models.CharField('作者/来源', max_length=100, blank=True)

    class Meta:
        verbose_name = '旅游资讯'
        verbose_name_plural = verbose_name
