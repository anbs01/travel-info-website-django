import sys
import io
import json
from psd_tools import PSDImage

# 强制输出为 UTF-8，解决 Windows 环境下的编码冲突
sys.stdout = io.TextIOWrapper(sys.stdout.detach(), encoding='utf-8')

def probe_metadata(psd_path):
    psd = PSDImage.open(psd_path)
    font_names = set()
    
    for layer in psd.descendants():
        if layer.kind == 'type':
            # 方法 1: 从 ResourceDict 获取
            try:
                font_set = layer.resource_dict['FontSet']
                for font in font_set:
                    font_names.add(str(font['Name']))
            except:
                pass
            
            # 方法 2: 从 EngineDict 获取 (更底层)
            try:
                font_set = layer.engine_dict['ResourceDict']['FontSet']
                for font in font_set:
                    font_names.add(str(font['Name'] if isinstance(font, dict) else font))
            except:
                pass
                
    return sorted(list(font_names))

if __name__ == "__main__":
    results = probe_metadata(r"docs/原始资料/设计页/首页.psd")
    print(json.dumps(results, indent=2, ensure_ascii=False))
