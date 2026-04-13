from django.db import models
from core.models import BaseContent

class Food(BaseContent):
    """特色美食模型"""
    recommend_level = models.IntegerField('推荐指数', default=5, help_text='1-5星')
    price_range = models.CharField('参考价格', max_length=50, blank=True)
    taste_profile = models.CharField('口味特点', max_length=100, blank=True)

    class Meta:
        verbose_name = '特色美食'
        verbose_name_plural = verbose_name
