from psd_tools import PSDImage
import json
import sys
import os

def get_all_fonts(psd_path):
    if not os.path.exists(psd_path):
        return {"error": f"File not found: {psd_path}"}
    
    psd = PSDImage.open(psd_path)
    found_fonts = set()
    
    for layer in psd.descendants():
        if layer.kind == 'type':
            try:
                # 尝试从 EngineDict 读取解析后的字体集
                font_set = layer.engine_dict['ResourceDict']['FontSet']
                for font in font_set:
                    name = font.get('Name')
                    if name:
                        found_fonts.add(name)
            except (KeyError, AttributeError):
                continue
    
    return sorted(list(found_fonts))

if __name__ == "__main__":
    target = sys.argv[1] if len(sys.argv) > 1 else r"docs/原始资料/设计页/首页.psd"
    fonts = get_all_fonts(target)
    print(json.dumps(fonts, indent=2, ensure_ascii=False))
