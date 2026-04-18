from django.urls import path
from .views import GoodListView, GoodDetailView

app_name = 'goods'

urlpatterns = [
    path('', GoodListView.as_view(), name='list'),
    path('g<int:offset_id>/', GoodDetailView.as_view(), name='detail'),
]
