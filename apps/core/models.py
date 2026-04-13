from django.db import models
from ckeditor.fields import RichTextField

class BaseContent(models.Model):
    """内容抽象基类"""
    WATERMARK_POS_CHOICES = [
        (0, '无'),
        (1, '左上'),
        (2, '右上'),
        (3, '居中'),
        (4, '左下'),
        (5, '右下'),
    ]

    title = models.CharField('标题', max_length=200)
    summary = models.TextField('摘要', blank=True)
    cover_image = models.ImageField('封面图', upload_to='covers/%Y/%m/', blank=True)
    content = RichTextField('内容详情')
    
    # 发布属性
    views = models.PositiveIntegerField('点击量', default=0)
    is_hidden = models.BooleanField('是否屏蔽', default=False)
    is_top = models.BooleanField('是否置顶', default=False)
    sort_order = models.IntegerField('排序权重', default=0, help_text='数值越大越靠前')
    
    # 功能属性
    redirect_url = models.URLField('跳转链接', blank=True, help_text='填写则点击直接跳转')
    watermark_pos = models.IntegerField('水印位置', choices=WATERMARK_POS_CHOICES, default=5)
    
    # 时间戳
    created_at = models.DateTimeField('创建时间', auto_now_add=True)
    updated_at = models.DateTimeField('更新时间', auto_now=True)

    class Meta:
        abstract = True
        ordering = ['-is_top', '-sort_order', '-created_at']

    def __str__(self):
        return self.title
