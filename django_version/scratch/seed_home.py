import os
import django
import sys

# 设置 Django 环境
sys.path.append(os.getcwd())
sys.path.append(os.path.join(os.getcwd(), 'apps'))
os.environ.setdefault('DJANGO_SETTINGS_MODULE', 'config.settings')
django.setup()

from core.models import HomeTextRecommend, SearchKeyword
from travelogue.models import Travelogue
from places.models import Place, Region

def seed():
    print("Clearing old data...")
    HomeTextRecommend.objects.all().delete()
    SearchKeyword.objects.all().delete()
    Travelogue.objects.all().delete()

    print("Seeding search keywords...")
    for kw in ['历史文化名城', '宜居城市', '特别推荐', '红色旅游']:
        SearchKeyword.objects.get_or_create(keyword=kw)

    print("Seeding text recommendations...")
    recommends = [
        ('设置在首页推荐的文字记录，不取消或未到期会一直显示在最左边', '#'),
        ('台湾写真：台北寻“杠子头”，咬一口“硬邦... [纪行]', '#'),
        ('“海绵”常德，一条老街留住文化与乡愁 [攻略]', '#'),
        ('打卡青岛明月山海间不夜城，全攻略带你深度游... [资讯]', '#'),
        ('周末，在岱山来场Citywalk！ [纪行]', '#'),
        ('绮罗古镇：六百年岁月里的古村肌理与人文... [攻略]', '#'),
        ('超600年历史！广州这条条避世古村，藏得太... [资讯]', '#'),
        ('江西省景德镇市浮梁县瑶里村：古村白墙黛... [纪行]', '#'),
    ]
    for i, (title, url) in enumerate(recommends):
        HomeTextRecommend.objects.create(title=title, url=url, sort_order=len(recommends)-i)

    print("Seeding featured travelogue...")
    r, _ = Region.objects.get_or_create(name='山东')
    p, _ = Place.objects.get_or_create(
        title='烟台', 
        region=r, 
        english_code='yantai'
    )
    Travelogue.objects.create(
        title='爱在烟台，难以离开',
        summary='烟台海滨三日游记',
        content='''
            <p>烟台，这座依山傍水、独具魅力的海滨城市，总是能给人留下深刻的印象。在这里，你可以感受海风的轻拂，聆听浪花的歌唱。</p>
            <p>我们的行程从滨海广场开始。早晨的广场空气清新，许多当地人在晨练。沿着海边木栈道漫步，右侧是蔚蓝的大海，左侧是现代化的都市建筑，这种和谐共生的美感令人陶醉。</p>
            <p>接着，我们来到了烟台山的灯塔。登上塔顶，俯瞰整个烟台港，船泊忙碌地进出，展示着这座港口城市的非凡活力。</p>
            <p>当然，烟台的美食也是不容错过的。新鲜的海鲜，地道的鲁菜，每一口都是对味蕾的极致犒劳。</p>
        ''',
        place=p,
        is_home=True,
        is_sticky=True
    )
    print("Seeding complete!")

if __name__ == '__main__':
    seed()
