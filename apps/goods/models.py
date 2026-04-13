from django.db import models
from core.models import BaseContent

class Good(BaseContent):
    """地方特产模型"""
    origin_place = models.CharField('产地', max_length=100, blank=True)
    storage_method = models.CharField('保存方式', max_length=100, blank=True)
    is_recommend_gift = models.BooleanField('是否推荐伴手礼', default=True)

    class Meta:
        verbose_name = '地方特产'
        verbose_name_plural = verbose_name
