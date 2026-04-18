from django.urls import path
from .views import FoodListView, FoodDetailView

app_name = 'foods'

urlpatterns = [
    path('', FoodListView.as_view(), name='list'),
    path('f<int:offset_id>/', FoodDetailView.as_view(), name='detail'),
]
