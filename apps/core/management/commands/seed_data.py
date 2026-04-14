import datetime
from django.core.management.base import BaseCommand
from django.db import transaction
from django.utils import timezone
from places.models import Region, Place, ScenicSpot, Traffic
from news.models import News
from travelogue.models import Travelogue
from foods.models import Food
from goods.models import Good
from core.models import Feedback, SearchKeyword, HomeTextRecommend

class Command(BaseCommand):
    help = '填充旅游项目的初始化种子测试数据'

    def add_arguments(self, parser):
        parser.add_argument(
            '--clear',
            action='store_true',
            help='在填充前清空现有数据',
        )

    def handle(self, *args, **options):
        if options['clear']:
            self.stdout.write('正在清理现有数据...')
            Feedback.objects.all().delete()
            News.objects.all().delete()
            Travelogue.objects.all().delete()
            Food.objects.all().delete()
            Good.objects.all().delete()
            ScenicSpot.objects.all().delete()
            Traffic.objects.all().delete()
            Place.objects.all().delete()
            Region.objects.all().delete()
            SearchKeyword.objects.all().delete()
            HomeTextRecommend.objects.all().delete()

        self.stdout.write('开始填充种子数据...')

        with transaction.atomic():
            # 1. 创建区域 (Regions)
            zj, _ = Region.objects.get_or_create(
                name='浙江',
                full_name='浙江省',
                introduction='浙江省地处中国东南沿海，长江三角洲南翼。',
                sort_order=1
            )
            sh, _ = Region.objects.get_or_create(
                name='上海',
                full_name='上海市',
                introduction='上海，简称“沪”，是中华人民共和国省级行政区、直辖市。',
                sort_order=2
            )

            # 2. 创建城乡 (Places)
            wuzhen, _ = Place.objects.get_or_create(
                title='乌镇',
                region=zj,
                english_code='wuzhen',
                defaults={
                    'full_title': '桐乡市乌镇镇',
                    'summary': '乌镇是典型的江南水乡古镇，素有“鱼米之乡，丝绸之府”之称。',
                    'level_tag': '国家 AAAAA 级景区',
                    'feature_tags': '江南水乡,千年古镇,摇船,夜游,戏剧节',
                    'alias': '似水年华',
                    'best_time': '3月-5月, 10月-11月',
                    'publish_date': timezone.now().date(),
                }
            )

            xitang, _ = Place.objects.get_or_create(
                title='西塘',
                region=zj,
                english_code='xitang',
                defaults={
                    'full_title': '嘉善县西塘古镇',
                    'summary': '西塘古镇是江南六大古镇之一，其“廊棚”建筑极具特色。',
                    'level_tag': 'AAAAA 级',
                    'feature_tags': '烟雨长廊,古镇生活,文化遗产',
                    'alias': '生活着的千年古镇',
                    'best_time': '4月-10月',
                    'publish_date': timezone.now().date(),
                }
            )

            # 3. 美食 (Foods)
            Food.objects.get_or_create(
                title='乌镇酱羊肉',
                place=wuzhen,
                defaults={
                    'level_tag': '特色风味',
                    'feature_tags': '软糯,滋补,历史悠久',
                    'price': 88.00,
                    'summary': '乌镇酱羊肉是乌镇历史悠久的传统名菜，入口即化，香气四溢。',
                    'content': '<p>乌镇酱羊肉选用当地湖羊，配以特制酱料焖煮而成。</p>',
                }
            )

            # 4. 文创 (Goods)
            Good.objects.get_or_create(
                title='蓝印花布',
                place=wuzhen,
                defaults={
                    'level_tag': '国家级非遗',
                    'feature_tags': '传统工艺,民俗文化',
                    'price': 120.00,
                    'summary': '蓝印花布是乌镇的传统手工艺品，以其独特的蓝白设计著称。',
                    'content': '<p>蓝印花布，又名“印花布”，是乌镇最具代表性的非遗产品之一。</p>',
                }
            )

            # 5. 景区 (ScenicSpots)
            ScenicSpot.objects.get_or_create(
                title='乌镇西栅景区',
                place=wuzhen,
                english_code='wuzhen-xizha',
                defaults={
                    'spot_type': 'scenic',
                    'level_tag': '核心景区',
                    'feature_tags': '夜景,艺术馆,木屋',
                    'summary': '西栅由12座小岛组成，70多座小桥将这些小岛串连在一起。',
                }
            )

            # 6. 交通 (Traffic)
            Traffic.objects.get_or_create(
                title='乌镇汽车站',
                place=wuzhen,
                defaults={
                    'traffic_type': '汽车站',
                    'summary': '乌镇镇中心主要客运交通枢纽，通往嘉兴、杭州、苏州等地。',
                }
            )

            # 7. 资讯 (News)
            News.objects.get_or_create(
                title='第九届乌镇戏剧节盛大开幕',
                defaults={
                    'slug': '2024戏剧节专报',
                    'level_tag': '行业盛会',
                    'feature_tags': '戏剧,文化,艺术',
                    'summary': '来自全球的戏剧爱好者齐聚乌镇，开启为期10天的艺术盛宴。',
                    'content': '<p>本届戏剧节规模再创新高...</p>',
                    'is_sticky': True,
                    'sticky_at': timezone.now(),
                    'place': wuzhen,
                }
            )

            # 8. 攻略 (Travelogues)
            Travelogue.objects.get_or_create(
                title='避开人流！乌镇西栅24小时漫游指南',
                defaults={
                    'slug': '攻略001',
                    'author': '旅行达人小王',
                    'level_tag': '金牌攻略',
                    'feature_tags': '深度旅游,摄影建议',
                    'summary': '如何用24小时玩转西栅最精华的景点和隐藏机位。',
                    'content': '<p>第一站：草本染色作坊...</p>',
                    'place': wuzhen,
                }
            )

            # 9. 反馈 (Feedback)
            Feedback.objects.create(
                title='建议在详情页增加语音讲解',
                content='如果在扫描二维码或者是进入页面后能有自动播放的语音导览就更好了。',
                name='张先生',
                contact='138****0001',
            )
            Feedback.objects.create(
                title='地图加载速度较慢',
                content='在查看交通枢纽地图时，图片加载时间有点久，希望能优化。',
                name='匿名游客',
                contact='feedback@test.com',
            )

            # 10. 关键词与首页推荐
            SearchKeyword.objects.get_or_create(keyword='非遗文化', sort_order=1)
            SearchKeyword.objects.get_or_create(keyword='江南古镇', sort_order=2)
            
            HomeTextRecommend.objects.get_or_create(
                title='2024最值得去的十大古镇排行榜',
                url='https://example.com/top10',
                sort_order=1
            )

        self.stdout.write(self.style.SUCCESS('成功填充所有种子数据！'))
