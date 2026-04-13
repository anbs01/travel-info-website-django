from django.db import models
from core.models import BaseContent

class Travelogue(BaseContent):
    """游记攻略模型"""
    DIFFICULTY_CHOICES = [
        (1, '轻松'),
        (2, '适中'),
        (3, '硬核'),
    ]

    difficulty = models.IntegerField('推荐难度', choices=DIFFICULTY_CHOICES, default=1)
    travel_days = models.PositiveIntegerField('建议天数', default=1)
    best_season = models.CharField('最佳季节', max_length=100, blank=True)

    class Meta:
        verbose_name = '游记攻略'
        verbose_name_plural = verbose_name
