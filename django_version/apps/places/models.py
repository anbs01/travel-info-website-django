from django.db import models
from core.models import BaseContent

class Region(models.Model):
    """行政区域/省份/国家"""
    name = models.CharField('名称', max_length=100)
    full_name = models.CharField('全称', max_length=200, blank=True)
    introduction = models.TextField('区域介绍', blank=True)
    english_name = models.CharField('英文名称', max_length=100, blank=True)
    is_active = models.BooleanField('是否启用', default=True)
    sort_order = models.PositiveIntegerField('排序', default=0)

    class Meta:
        verbose_name = '行政区域'
        verbose_name_plural = verbose_name
        ordering = ['-sort_order', 'name']

    def __str__(self):
        return self.name

class Place(BaseContent):
    """城乡/街巷 - 灵魂基准版"""
    region = models.ForeignKey(Region, on_delete=models.CASCADE, verbose_name='所属区域')
    english_code = models.SlugField('英文标识(Slug)', max_length=100, unique=True, help_text='用于URL路由，如: hangzhou')
    
    # 设计图补强字段
    alias = models.CharField('别名', max_length=100, blank=True, help_text='如: 泉城')
    best_time = models.CharField('最佳游玩时间', max_length=200, blank=True, help_text='如: 4月-10月')
    parent = models.ForeignKey('self', on_delete=models.SET_NULL, null=True, blank=True, related_name='sub_places', verbose_name='上级城乡')

    class Meta:
        verbose_name = '城乡/街巷'
        verbose_name_plural = verbose_name

class ScenicSpot(BaseContent):
    """热门打卡地/景区"""
    place = models.ForeignKey(Place, on_delete=models.CASCADE, verbose_name='所属城镇乡村')
    spot_type = models.CharField('地点类型', max_length=20, choices=[('scenic', '景区'), ('checkin', '打卡点')], default='scenic')
    english_code = models.SlugField('英文标识(Slug)', max_length=100, unique=True, help_text='用于URL路由')
    address = models.CharField('详细地址', max_length=255, blank=True)
    opening_hours = models.CharField('开放时间', max_length=200, blank=True)
    ticket_info = models.CharField('门票信息', max_length=200, blank=True)

    class Meta:
        verbose_name = '打卡地/景区'
        verbose_name_plural = verbose_name

class Traffic(BaseContent):
    """交通出行"""
    place = models.ForeignKey(Place, on_delete=models.CASCADE, verbose_name='目的地城乡')
    traffic_type = models.CharField('交通方式', max_length=50, help_text='如: 高铁, 飞机, 自驾')
    duration = models.CharField('耗时/说明', max_length=100, blank=True)

    class Meta:
        verbose_name = '交通出行'
        verbose_name_plural = verbose_name
