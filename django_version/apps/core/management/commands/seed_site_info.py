from django.core.management.base import BaseCommand
from core.models import SiteInfo

class Command(BaseCommand):
    help = '初始化网站基本信息 (SiteInfo)'

    def handle(self, *args, **options):
        # 获取或创建单例记录
        site_info, created = SiteInfo.objects.get_or_create(id=1)
        
        site_info.about = """
            <h2>探索未知的旅程，记录最美的瞬间</h2>
            <p>旅游信息网（NAME.COM）致力于为每一位旅行者提供最真实、最深度的在地文化探索指南。我们相信，每一次出发都是一场灵魂的洗礼。</p>
            <p>在这里，您可以发现隐藏在城乡巷弄间的非遗魅力，品尝最正宗的家乡味道，搜寻极具匠心的文创好物。我们不仅是信息的聚合者，更是旅行美学的倡导者。</p>
        """
        
        site_info.service = """
            <h2>用户服务协议</h2>
            <p>欢迎使用旅游信息网提供的各项服务。请您在注册或开始使用本服务前，务必仔细阅读并理解本协议的所有条款。</p>
            <p>1. <b>服务内容：</b>本站提供旅游资讯浏览、攻略分享及配套信息索引服务。</p>
            <p>2. <b>免责声明：</b>本站信息部分来源于官方、部分由用户贡献，使用相关信息时请自行核实，本站不承担因信息误差导致的直接或间接损失。</p>
        """
        
        site_info.cooperation = """
            <h2>携手同行，共创美好</h2>
            <p>我们热忱欢迎旅游局、景区、非遗传承人及文创品牌与我们达成深度合作。通过多维度内容挖掘与展示，共同提升文化旅游品牌的商业价值与社会影响力。</p>
            <p><b>合作方式：</b>专题策划、深度报道、品牌落地、数据推流等。</p>
        """
        
        site_info.contact = """
            <h2>联系我们</h2>
            <p>如果您在浏览过程中有任何疑问、建议或合作意向，欢迎通过以下方式与我们取得联系：</p>
            <ul>
                <li><b>联系地址：</b>山东省济南市历下区泉城路88号</li>
                <li><b>客服电话：</b>400-123-4567 (工作日 9:00-18:00)</li>
                <li><b>企业邮箱：</b>contact@name.com</li>
                <li><b>官方微信：</b>旅游信息网订阅号</li>
            </ul>
        """
        
        site_info.copyright = "Copyright © 2026-2027 NAME.COM 鲁ICP备123456789号"
        site_info.copyright_home = "© 2026 NAME.COM 鲁ICP备123456789号 | 追求卓越 记录真实"
        
        site_info.save()
        
        if created:
            self.stdout.write(self.style.SUCCESS('成功初始化 SiteInfo 数据'))
        else:
            self.stdout.write(self.style.SUCCESS('成功更新 SiteInfo 默认文案'))
