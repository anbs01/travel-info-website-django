from django.urls import path
from .views import GoodListView, GoodDetailView

app_name = 'goods'

urlpatterns = [
    path('', GoodListView.as_view(), name='list'),
    path('<str:slug>/', GoodDetailView.as_view(), name='detail'),
]
