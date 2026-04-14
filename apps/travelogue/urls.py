from django.urls import path
from .views import TravelogueDetailView, TravelogueListView

app_name = 'travelogue'

urlpatterns = [
    path('', TravelogueListView.as_view(), name='list'),
    path('<slug:slug>/', TravelogueDetailView.as_view(), name='detail'),
]
