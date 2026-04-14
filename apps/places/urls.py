from django.urls import path
from .views import PlaceListView, PlaceDetailView

app_name = 'places'

urlpatterns = [
    path('', PlaceListView.as_view(), name='list'),
    path('<str:slug>/', PlaceDetailView.as_view(), name='detail'),
]
