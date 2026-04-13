from django.db import models
from django.utils import timezone
from core.models import BaseContent
from places.models import Place

class Travelogue(BaseContent):
    """游记攻略模型 - 文档 3.2 版"""
    slug = models.CharField('URL缩略名', max_length=20, unique=True, help_text='发布时自动生成（YYYYMMDDHHmm）')
    source = models.CharField('来源', max_length=100, blank=True)
    author = models.CharField('作者', max_length=100, blank=True)
    content = models.TextField('富文本正文')
    place = models.ForeignKey(Place, on_delete=models.SET_NULL, null=True, blank=True, related_name='travelogues', verbose_name='关联城乡')

    class Meta:
        verbose_name = '游记攻略'
        verbose_name_plural = verbose_name

    def save(self, *args, **kwargs):
        if not self.slug:
            base_slug = timezone.now().strftime('%Y%M%d%H%M')
            if Travelogue.objects.filter(slug=base_slug).exists():
                base_slug = timezone.now().strftime('%Y%M%d%H%M%S')[-14:]
            self.slug = base_slug
        super().save(*args, **kwargs)
