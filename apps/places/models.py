from django.db import models
from core.models import BaseContent

class Place(BaseContent):
    """地区与打卡地模型"""
    PLACE_TYPE_CHOICES = [
        ('city', '城市'),
        ('scenic', '景区'),
        ('spot', '打卡点'),
    ]

    parent = models.ForeignKey('self', on_delete=models.SET_NULL, null=True, blank=True, related_name='children', verbose_name='上级地区')
    place_type = models.CharField('地点类型', max_length=10, choices=PLACE_TYPE_CHOICES, default='city')
    
    # 扩展字段
    address = models.CharField('详细地址', max_length=255, blank=True)
    phone = models.CharField('联系电话', max_length=50, blank=True)
    opening_hours = models.CharField('开放时间', max_length=200, blank=True)
    ticket_price = models.CharField('门票信息', max_length=100, blank=True)

    class Meta:
        verbose_name = '地区打卡地'
        verbose_name_plural = verbose_name

class PlaceImage(models.Model):
    """打卡地相册"""
    place = models.ForeignKey(Place, on_delete=models.CASCADE, related_name='images')
    image = models.ImageField('图片', upload_to='places/%Y/%m/')
    description = models.CharField('图片描述', max_length=200, blank=True)
    sort_order = models.IntegerField('排序系数', default=0)

    class Meta:
        verbose_name = '打卡地图片'
        verbose_name_plural = verbose_name
        ordering = ['sort_order']
