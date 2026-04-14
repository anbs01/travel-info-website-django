from django.urls import path
from . import views
from .views import IndexView, DetailSampleView, GlobalSearchView

app_name = 'core'

urlpatterns = [
    path('', IndexView.as_view(), name='index'),
    path('search/', views.GlobalSearchView.as_view(), name='global_search'),
    
    # 静态专题页 (URL映射至同一个视图)
    path('about/', views.InfoPageView.as_view(), {'page_type': 'about'}, name='about'),
    path('service/', views.InfoPageView.as_view(), {'page_type': 'service'}, name='service'),
    path('cooperation/', views.InfoPageView.as_view(), {'page_type': 'cooperation'}, name='cooperation'),
    path('contact/', views.InfoPageView.as_view(), {'page_type': 'contact'}, name='contact'),
    
    # 互动反馈
    path('feedback/add/', views.FeedbackView.as_view(), name='feedback_add'),
    path('detail-sample/', DetailSampleView.as_view(), name='detail_sample'),
]
