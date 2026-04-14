from django.db import models
from core.models import BaseContent
from places.models import Place

class Good(BaseContent):
    """好物·文创 - 灵魂基准版"""
    place = models.ForeignKey(Place, on_delete=models.CASCADE, verbose_name='所属城镇乡村')
    english_code = models.SlugField('英文标识(Slug)', max_length=100, unique=True)
    price = models.DecimalField('价格/参考价', max_digits=10, decimal_places=2, null=True, blank=True)
    
    # level_tag 用于存储“非遗级别”
    # feature_tags 用于存储“产品特色”

    class Meta:
        verbose_name = '好物·文创'
        verbose_name_plural = verbose_name
