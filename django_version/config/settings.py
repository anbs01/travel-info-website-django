from pathlib import Path
import os
import sys
import environ

# Initialize environ
env = environ.Env(
    DEBUG=(bool, False)
)

# Build paths inside the project like this: BASE_DIR / 'subdir'.
BASE_DIR = Path(__file__).resolve().parent.parent

# Read .env file
environ.Env.read_env(os.path.join(BASE_DIR, '.env'))

# Add apps directory to sys.path
sys.path.insert(0, os.path.join(BASE_DIR, 'apps'))


# Quick-start development settings - unsuitable for production
# See https://docs.djangoproject.com/en/6.0/howto/deployment/checklist/

# SECURITY WARNING: keep the secret key used in production secret!
SECRET_KEY = env('SECRET_KEY')

# SECURITY WARNING: don't run with debug turned on in production!
DEBUG = env('DEBUG')

ALLOWED_HOSTS = ['*']


# Application definition

INSTALLED_APPS = [
    'simpleui',
    'django.contrib.admin',
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',
    
    # 3rd party apps
    'ckeditor',
    'django_cleanup.apps.CleanupConfig',
    
    # Local apps
    'core',
    'places',
    'news',
    'travelogue',
    'foods',
    'goods',
]

MIDDLEWARE = [
    'django.middleware.security.SecurityMiddleware',
    'django.contrib.sessions.middleware.SessionMiddleware',
    'django.middleware.common.CommonMiddleware',
    'django.middleware.csrf.CsrfViewMiddleware',
    'django.contrib.auth.middleware.AuthenticationMiddleware',
    'django.contrib.messages.middleware.MessageMiddleware',
    'django.middleware.clickjacking.XFrameOptionsMiddleware',
]

ROOT_URLCONF = 'config.urls'

TEMPLATES = [
    {
        'BACKEND': 'django.template.backends.django.DjangoTemplates',
        'DIRS': [os.path.join(BASE_DIR, 'templates')],
        'APP_DIRS': True,
        'OPTIONS': {
            'context_processors': [
                'django.template.context_processors.debug',
                'django.template.context_processors.request',
                'django.contrib.auth.context_processors.auth',
                'django.contrib.messages.context_processors.messages',
                'django.template.context_processors.media',
            ],
        },
    },
]

WSGI_APPLICATION = 'config.wsgi.application'


# Database
# https://docs.djangoproject.com/en/6.0/ref/settings/#databases

DATABASES = {
    'default': env.db(),
}


# Password validation
# https://docs.djangoproject.com/en/6.0/ref/settings/#auth-password-validators

AUTH_PASSWORD_VALIDATORS = [
    {
        'NAME': 'django.contrib.auth.password_validation.UserAttributeSimilarityValidator',
    },
    {
        'NAME': 'django.contrib.auth.password_validation.MinimumLengthValidator',
    },
    {
        'NAME': 'django.contrib.auth.password_validation.CommonPasswordValidator',
    },
    {
        'NAME': 'django.contrib.auth.password_validation.NumericPasswordValidator',
    },
]


# Internationalization
# https://docs.djangoproject.com/en/6.0/topics/i18n/

LANGUAGE_CODE = 'zh-hans'

TIME_ZONE = 'Asia/Shanghai'

USE_I18N = True

USE_TZ = True


# Static files (CSS, JavaScript, Images)
# https://docs.djangoproject.com/en/6.0/howto/static-files/

STATIC_URL = 'static/'
STATICFILES_DIRS = [os.path.join(BASE_DIR, 'static')]
STATIC_ROOT = os.path.join(BASE_DIR, 'staticfiles')

MEDIA_URL = 'media/'
MEDIA_ROOT = os.path.join(BASE_DIR, 'media')

# Default primary key field type
# https://docs.djangoproject.com/en/6.0/ref/settings/#default-auto-field

DEFAULT_AUTO_FIELD = 'django.db.models.BigAutoField'

# Session timeout (30 minutes)
SESSION_COOKIE_AGE = 1800
SESSION_EXPIRE_AT_BROWSER_CLOSE = True

# SimpleUI Settings
SIMPLEUI_HOME_INFO = False  # 隐藏官方分析页
SIMPLEUI_ANALYSIS = False   # 隐藏分析统计

# 像素级还原菜单配置 (完全对齐设计图：网站名称.png)
SIMPLEUI_CONFIG = {
    'system_keep': False,  # 完全接管系统菜单，自定义排序
    'system_keep_alive': False,  # 禁用 iframe 模式，恢复原生稳定性
    'menu_display': ['综合管理', '纪行攻略', '行业资讯', '城镇乡村', '打卡地', '特产美食', '文创商品', '系统管理'],
    'menus': [
        {
            'name': '综合管理',
            'icon': 'fas fa-desktop',
            'models': [
                {'name': '互动信息(2)', 'icon': 'fas fa-comments', 'url': 'core/feedback/'},
                {'name': '类别热词', 'icon': 'fas fa-tags', 'url': 'core/searchkeyword/'},
                {'name': '版权引用', 'icon': 'fas fa-copyright', 'url': 'core/siteinfo/'},
                {'name': '基本信息', 'icon': 'fas fa-info-circle', 'url': 'core/siteinfo/'},
                {'name': '首页推荐', 'icon': 'fas fa-thumbs-up', 'url': 'core/hometextrecommend/'},
            ]
        },
        {
            'name': '纪行攻略',
            'icon': 'fas fa-user-cog',
            'url': 'travelogue/travelogue/'
        },
        {
            'name': '行业资讯',
            'icon': 'fas fa-cloud',
            'url': 'news/news/'
        },
        {
            'name': '城镇乡村',
            'icon': 'fas fa-th-list',
            'url': 'places/place/'
        },
        {
            'name': '打卡地',
            'icon': 'fas fa-university',
            'url': 'places/scenicspot/'
        },
        {
            'name': '特产美食',
            'icon': 'fas fa-utensils',
            'url': 'foods/food/'
        },
        {
            'name': '文创商品',
            'icon': 'fas fa-gift',
            'url': 'goods/good/'
        },
        {
            'name': '系统管理',
            'icon': 'fas fa-cogs',
            'models': [
                {'name': '用户', 'icon': 'fas fa-user', 'url': 'auth/user/'},
                {'name': '组', 'icon': 'fas fa-users-cog', 'url': 'auth/group/'},
            ]
        }
    ]
}

# 自定义图标配置 (用于非菜单区域的备用显示)
SIMPLEUI_ICON = {
    '网站基本信息': 'fas fa-info-circle',
    '搜索关键词': 'fas fa-search',
    '首页文字推荐': 'fas fa-thumbs-up',
    '特产·美食': 'fas fa-utensils',
    '好物·文创': 'fas fa-shopping-bag',
    '资讯·动态': 'fas fa-newspaper',
    '行政区域': 'fas fa-globe-asia',
    '城乡/街巷': 'fas fa-city',
    '打卡地/景区': 'fas fa-camera-retro',
    '交通出行': 'fas fa-bus',
    '游记攻略': 'fas fa-paper-plane',
}

# CKEditor 配置 (对齐需求文档 4.0 & 15.3)
CKEDITOR_UPLOAD_PATH = 'admin/%Y%m/'  # 自动按年月分目录
CKEDITOR_IMAGE_BACKEND = 'pillow'
CKEDITOR_CONFIGS = {
    'default': {
        'toolbar': 'Full',
        'height': 400,
        'width': 'auto',
        # 激活拖拽上传与图片等比缩放
        'extraPlugins': ','.join([
            'uploadimage',
            'image2',
            'widget',
            'lineutils',
        ]),
    },
}
