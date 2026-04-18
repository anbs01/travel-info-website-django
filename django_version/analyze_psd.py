from psd_tools import PSDImage
import json
import os

def snap_to_grid(value):
    """
    智能吸附逻辑：
    1. 优先向 10 的倍数吸附 (误差 <= 2px)
    2. 其次向 4/8 的倍数吸附 (误差 <= 1px)
    """
    if value == 0: return 0
    # 尝试整十吸附
    targets = [round(value / 10) * 10, round(value / 4) * 4]
    for t in targets:
        if abs(value - t) <= 2:
            return t
    return value

def analyze_psd_layers(layer_or_psd, depth=0):
    results = []
    if hasattr(layer_or_psd, 'width') and depth == 0:
        items = layer_or_psd
    else:
        items = layer_or_psd if hasattr(layer_or_psd, '__iter__') else []

    for layer in items:
        raw_rect = [layer.left, layer.top, layer.width, layer.height]
        clean_rect = [snap_to_grid(x) for x in raw_rect]
        
        info = {
            "name": layer.name,
            "type": "group" if layer.is_group() else "layer",
            "rect": clean_rect,
            "raw_rect": raw_rect,
            "visible": layer.visible,
            "opacity": layer.opacity / 255.0 if hasattr(layer, 'opacity') else 1.0
        }

        # 维度 1: 字体排版 (Typography)
        if layer.kind == 'type':
            try:
                text_data = layer.engine_dict
                info["text_content"] = layer.text
            
                # 增强版字体提取逻辑
                try:
                    font_family = "Unknown"
                    # 路径 A: ResourceDict (推荐)
                    if 'ResourceDict' in layer.engine_dict:
                        font_set = layer.engine_dict['ResourceDict']['FontSet']
                        run = layer.engine_dict['StyleRun']['RunArray'][0]
                        font_index = run['StyleSheet']['StyleSheetData']['Font']
                        font_family = str(font_set[font_index]['Name'])
                    # 路径 B: 直接从 FontSet 提取 (回退)
                    elif hasattr(layer, 'resource_dict') and 'FontSet' in layer.resource_dict:
                        font_family = str(list(layer.resource_dict['FontSet'])[0]['Name'])
                    
                    info["font_family"] = font_family
                    
                    # 提取文字样式
                    run = layer.engine_dict['StyleRun']['RunArray'][0]
                    font_info = run['StyleSheet']['StyleSheetData']
                    info["font_size"] = round(font_info.get('FontSize', 12), 1)
                    rgb = font_info.get('FillColor', {}).get('Values', [0, 0, 0, 1])
                    info["color"] = '#{:02x}{:02x}{:02x}'.format(int(rgb[1]*255), int(rgb[2]*255), int(rgb[3]*255))
                except Exception:
                    info["font_family"] = "MicrosoftYaHei"  # 最后的默认回退
                    info["font_size"] = 12
                    info["color"] = "#000000"
            except:
                info["text_content"] = getattr(layer, 'text', '')

        # 维度 3: 视觉特效 (Effects - 简化版)
        if hasattr(layer, 'effects'):
            effects = []
            for effect in layer.effects:
                if effect.enabled:
                    effects.append(effect.__class__.__name__)
            if effects: info["effects"] = effects

        if layer.is_group() and depth < 3: # 深度增加到 3 层以覆盖更多 UI 细节
            info["children"] = analyze_psd_layers(layer, depth + 1)
        results.append(info)
    return results

if __name__ == "__main__":
    import sys
    import os
    if len(sys.argv) > 1:
        target_psd = sys.argv[1]
    else:
        target_psd = r"d:\LQH\Git\travel-info-website-django\docs\原始资料\设计页\首页.psd"

    if not os.path.exists(target_psd):
        print(f"Error: File not found - {target_psd}")
        sys.exit(1)

    print(f"Analyzing: {target_psd}")
    psd = PSDImage.open(target_psd)
    data = {
        "filename": os.path.basename(target_psd),
        "canvas": [psd.width, psd.height],
        "structure": analyze_psd_layers(psd)
    }
    
    # 直接写入文件以规避终端编码报错
    # 自动生成对应的规格书文件名
    psd_filename = os.path.basename(target_psd).replace('.psd', '')
    output_path = os.path.join('docs', 'design_specs', f"{psd_filename}_spec.json")
    
    # 确保目录存在
    os.makedirs(os.path.dirname(output_path), exist_ok=True)
    
    with open(output_path, 'w', encoding='utf-8') as f:
        json.dump(data, f, indent=2, ensure_ascii=False)
    
    print(f"Success: Data saved to {output_path}")
