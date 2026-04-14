from django.urls import path
from .views import IndexView, DetailSampleView

app_name = 'core'

urlpatterns = [
    path('', IndexView.as_view(), name='index'),
    path('detail-sample/', DetailSampleView.as_view(), name='detail_sample'),
]
