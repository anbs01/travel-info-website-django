from django.urls import path
from .views import NewsListView, NewsDetailView

app_name = 'news'

urlpatterns = [
    path('', NewsListView.as_view(), name='list'),
    path('<str:slug>/', NewsDetailView.as_view(), name='detail'),
]
