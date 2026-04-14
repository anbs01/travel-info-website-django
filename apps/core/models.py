from django.db import models
from django.utils import timezone
from ckeditor_uploader.fields import RichTextUploadingField

class BaseContent(models.Model):
    """内容抽象基类 - 灵魂基准版 (高保真还原设计图)"""
    
    WATERMARK_POS_CHOICES = [
        ('center', '居中'),
        ('bottom_center', '底部中间'),
        ('bottom_right', '右下角'),
    ]

    # 1. 基础展示字段
    title = models.CharField('标题(简称)', max_length=200, help_text='用于列表、版面受限处')
    full_title = models.CharField('全称', max_length=200, blank=True, help_text='用于详情页展示')
    summary = models.TextField('内容概述', blank=True, help_text='详情页顶部的导语/特色说明')
    content = RichTextUploadingField('详细内容', blank=True, help_text='正文详情/富文本内容')
    
    # 2. 视觉灵魂字段 (补强)
    level_tag = models.CharField('级别/品别标签', max_length=50, blank=True, help_text='右上角突出显示的标签，如：非遗级别、口气、菜系等')
    feature_tags = models.CharField('特色标签云', max_length=255, blank=True, help_text='多个请用逗号隔开，如：亲子,观海,小火车')
    
    # 3. 媒体与水印
    image = models.ImageField('焦点图', upload_to='covers/%Y/%m/', blank=True)
    image_show_watermark = models.BooleanField('详情页显示水印', default=False)
    watermark_pos = models.CharField('水印位置', max_length=20, choices=WATERMARK_POS_CHOICES, default='bottom_right')
    
    # 4. 版权与来源 (补强)
    source = models.CharField('内容来源', max_length=100, blank=True, help_text='如：百度、官方提供')
    author = models.CharField('作者/贡献者', max_length=100, blank=True)
    publish_date = models.DateField('显示日期', null=True, blank=True, help_text='前台显示的自定义日期')
    
    # 5. 运营控制
    redirect_url = models.URLField('跳转链接', blank=True, help_text='填写后直接跳转')
    views = models.PositiveIntegerField('阅读量', default=0)
    is_sticky = models.BooleanField('是否置顶', default=False)
    sticky_at = models.DateTimeField('置顶时间', null=True, blank=True)
    is_home = models.BooleanField('首页推荐', default=False)
    is_hidden = models.BooleanField('隐藏屏蔽', default=False)
    
    # 6. 系统时间戳
    created_at = models.DateTimeField('添加时间', auto_now_add=True)
    updated_at = models.DateTimeField('修改时间', auto_now=True)

    class Meta:
        abstract = True
        ordering = ['-is_sticky', '-sticky_at', '-created_at']

    def __str__(self):
        return self.title

    def save(self, *args, **kwargs):
        if self.is_sticky and not self.sticky_at:
            self.sticky_at = timezone.now()
        super().save(*args, **kwargs)

class SiteInfo(models.Model):
    """网站基本信息 (单例)"""
    about = RichTextUploadingField('关于我们', blank=True)
    service = RichTextUploadingField('服务协议', blank=True)
    contact = RichTextUploadingField('联系我们', blank=True)
    cooperation = RichTextUploadingField('合作事宜', blank=True)
    copyright = models.TextField('版权信息(底部)', blank=True)
    copyright_home = models.TextField('首页专用版权信息', blank=True)

    class Meta:
        verbose_name = '网站基本信息'
        verbose_name_plural = verbose_name

class HomeTextRecommend(models.Model):
    """首页文字推荐"""
    title = models.CharField('推荐标题', max_length=200)
    url = models.URLField('跳转链接')
    is_active = models.BooleanField('是否启用', default=True)
    sort_order = models.PositiveIntegerField('排序', default=0)

    class Meta:
        verbose_name = '首页文字推荐'
        verbose_name_plural = verbose_name
        ordering = ['-sort_order']

class SearchKeyword(models.Model):
    """搜索关键词"""
    keyword = models.CharField('关键词', max_length=50)
    sort_order = models.PositiveIntegerField('排序', default=0)
    is_active = models.BooleanField('是否启用', default=True)

    class Meta:
        verbose_name = '搜索关键词'
        verbose_name_plural = verbose_name
        ordering = ['-sort_order']

class Feedback(models.Model):
    """互动信息 / 意见建议 - 补全设计图业务项"""
    title = models.CharField('标题/主题', max_length=200)
    content = models.TextField('正文内容')
    name = models.CharField('联系人', max_length=100, blank=True)
    contact = models.CharField('联系方式', max_length=100, blank=True, help_text='电话、邮箱或微信')
    is_processed = models.BooleanField('已处理', default=False)
    created_at = models.DateTimeField('提交时间', auto_now_add=True)

    class Meta:
        verbose_name = '互动信息'
        verbose_name_plural = verbose_name
        ordering = ['-created_at']

    def __str__(self):
        return self.title
