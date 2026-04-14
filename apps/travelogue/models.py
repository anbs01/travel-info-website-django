from django.db import models
from django.utils import timezone
from core.models import BaseContent
from places.models import Place

class Travelogue(BaseContent):
    """游记攻略 - 补全设计图业务项"""
    slug = models.CharField('URL码', max_length=20, unique=True, help_text='系统自动生成：YYYYMMDDHHmm格式')
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
