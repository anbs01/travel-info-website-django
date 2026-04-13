from django.db import models
from core.models import BaseContent
from places.models import Place

class Food(BaseContent):
    """特产·美食 - 灵魂基准版"""
    place = models.ForeignKey(Place, on_delete=models.CASCADE, verbose_name='归属城乡')
    english_code = models.SlugField('英文标识(Slug)', max_length=100, unique=True)
    
    # level_tag 用于存储“菜系统别”或“非遗级别”
    # feature_tags 用于存储“产品特色”

    class Meta:
        verbose_name = '特产·美食'
        verbose_name_plural = verbose_name
