# 需求文档

## 简介

本文档描述旅游信息门户网站（Django）的补全开发需求。该网站已有约 60-70% 的基础架构，包含 6 个 app（core、places、foods、goods、news、travelogue）、BaseContent 抽象基类、SimpleUI 后台菜单、CKEditor 富文本编辑器及基础列表视图。本次补全工作涵盖：URL 路由规范化、模型字段补充、详情页视图与模板、城乡综合主页逻辑、前端静态页面、首页逻辑修正、前端细节优化及后台管理增强。

---

## 词汇表

- **Portal**：本旅游信息门户网站系统整体
- **Place**：城乡/街巷实体，对应 `places.Place` 模型
- **Region**：行政区域/省份/国家，对应 `places.Region` 模型
- **ScenicSpot**：打卡地/景区实体，对应 `places.ScenicSpot` 模型
- **Traffic**：交通出行实体，对应 `places.Traffic` 模型
- **Travelogue**：游记攻略实体，对应 `travelogue.Travelogue` 模型
- **News**：资讯动态实体，对应 `news.News` 模型
- **Food**：特产美食实体，对应 `foods.Food` 模型
- **Good**：好物文创实体，对应 `goods.Good` 模型
- **Feedback**：意见建议/互动信息实体，对应 `core.Feedback` 模型
- **SiteInfo**：网站基本信息单例，对应 `core.SiteInfo` 模型
- **URL_Router**：Django URL 路由配置模块（`config/urls.py` 及各 app 的 `urls.py`）
- **View**：Django 视图类或函数
- **Template**：Django HTML 模板文件
- **Admin**：Django 后台管理界面（基于 SimpleUI）
- **Slug**：URL 中使用的唯一标识字符串
- **english_code**：Place 模型中用于 URL 的纯英文标识字段
- **place_type**：Place 模型中的城乡类型字段（地级市/县级市/乡镇/村庄）
- **traffic_type**：Traffic 模型中的交通方式字段（枚举选项）
- **F_Expression**：Django `F()` 表达式，用于数据库层面的原子更新操作
- **noimage.png**：无图片时的默认占位图（约 225×150 像素）
- **watermark.png**：水印图片（208×60 像素，最大宽度 960px 时使用）
- **qrcode.png**：网站二维码图片（展开尺寸 112×112 像素）

---

## 需求列表

### 需求 1：URL 路由规范化

**用户故事：** 作为网站访客，我希望每个内容页面都有语义化且稳定的 URL，以便收藏、分享和 SEO 优化。

#### 验收标准

1. WHEN 访客访问城乡详情页，THE URL_Router SHALL 将请求路由至 `/travelogue/ur{english_code}.shtml` 格式的 URL，其中 `english_code` 为 Place 的纯英文标识。
2. WHEN 访客访问攻略详情页，THE URL_Router SHALL 将请求路由至 `/travelogue/{YYYYmmddHHMM}.shtml` 格式的 URL，其中时间戳由创建时间生成。
3. WHEN 访客访问资讯详情页，THE URL_Router SHALL 将请求路由至 `/news/{YYYYmmddHHMM}.shtml` 格式的 URL。
4. WHEN 访客访问美食详情页，THE URL_Router SHALL 将请求路由至 `/foods/f{id}.shtml` 格式的 URL，其中 `id` 从 10001 起计（数据库 ID 加 10000 偏移）。
5. WHEN 访客访问文创详情页，THE URL_Router SHALL 将请求路由至 `/goods/g{id}.shtml` 格式的 URL，其中 `id` 从 10001 起计。
6. WHEN 访客访问景区/打卡地详情页，THE URL_Router SHALL 将请求路由至 `/travelogue/scenic{id}.shtml` 格式的 URL，其中 `id` 从 10001 起计。
7. WHEN 访客访问交通详情页，THE URL_Router SHALL 将请求路由至 `/travelogue/traffic{id}.shtml` 格式的 URL，其中 `id` 从 10001 起计。
8. IF 访客访问不存在或格式非法的 URL，THEN THE URL_Router SHALL 返回 HTTP 404 响应并渲染自定义 404 页面。
9. THE Travelogue 模型 SHALL 在 `save()` 方法中使用 `strftime('%Y%m%d%H%M')` 生成 slug（修正原有 `%M` 月份错误，`%m` 为月份，`%M` 为分钟）。
10. WHEN 内容被修改后，THE URL_Router SHALL 保持原有 URL 不变（假静态，slug 在首次创建后不再更新）。

---

### 需求 2：模型字段补充

**用户故事：** 作为后台管理员，我希望模型包含完整的业务字段，以便精确管理城乡分类、交通类型和海外标识。

#### 验收标准

1. THE Place 模型 SHALL 包含 `hide_nav` 布尔字段（默认 `False`），用于控制城乡详情页是否隐藏导航栏。
2. THE Place 模型 SHALL 包含 `place_type` 字段，取值范围为枚举选项：`city`（地级市）、`county`（县级市）、`town`（乡镇）、`village`（村庄），默认为 `city`。
3. THE Place 模型 SHALL 包含 `is_overseas` 布尔字段（默认 `False`），标识该城乡是否为海外城乡。
4. THE Region 模型 SHALL 包含 `is_overseas` 布尔字段（默认 `False`），标识该区域是否为海外区域。
5. THE Traffic 模型 SHALL 将 `traffic_type` 字段改为带 `choices` 的枚举字段，选项为：`airport`（机场）、`railway`（火车站）、`bus`（汽车站）、`water`（水上交通）、`metro`（轨道交通）、`road`（地面交通）。
6. WHEN 数据库迁移执行后，THE Admin SHALL 在城乡编辑表单中显示上述新增字段。

---

### 需求 3：城乡综合主页

**用户故事：** 作为网站访客，我希望城乡主页综合展示国内外城乡、打卡点和交通信息，并支持省份筛选和分页浏览。

#### 验收标准

1. THE PlaceListView SHALL 在城乡主页合并展示 Place（国内+海外）、ScenicSpot 和 Traffic 四类数据，每页最多显示 15 条。
2. THE PlaceListView SHALL 按以下优先级排序：首先显示各类置顶数据（按置顶时间倒序），其次显示普通数据（按添加时间倒序）。
3. WHEN 访客点击省份标签，THE PlaceListView SHALL 跳转至城乡搜索结果页（`/places/search/`），并传递省份参数。
4. WHEN 城乡主页首次加载，THE Template SHALL 默认弹出会员功能暂未上线的提示弹窗，该弹窗在 5 秒后自动关闭，也可手动关闭。
5. WHEN 访客勾选"不再提示"后关闭弹窗，THE Template SHALL 在浏览器本地存储（localStorage）中记录标记，后续访问不再弹出该弹窗（直至清除缓存）。
6. WHERE 城乡主页弹窗代码存在，THE Template SHALL 在弹窗代码块前后添加注释标记，便于后续移除。
7. WHEN 访客进行关键字搜索，THE PlaceListView SHALL 将搜索结果转至城乡搜索结果页（`/places/search/`）而非在主页内过滤。

---

### 需求 4：城乡搜索结果页

**用户故事：** 作为网站访客，我希望城乡搜索有独立的结果页，支持按省份和城乡类型筛选，并清晰展示搜索结果。

#### 验收标准

1. THE PlaceSearchView SHALL 在 `/places/search/` 路径提供独立的城乡搜索结果页。
2. WHEN 搜索结果为空，THE Template SHALL 显示"未找到相关城乡数据"的文字提示，且底部版权区域与提示文字之间保留足够间距。
3. WHEN 搜索结果不为空且非省份点击进入，THE Template SHALL 在结果列表上方显示"共找到 X 条相关结果"的文字提示。
4. WHEN 访客通过省份点击进入搜索页，THE Template SHALL 在页面顶部显示省份全称（国内省份显示全称，海外区域显示"海外地区"）。
5. WHEN 访客通过省份点击进入搜索页，THE Template SHALL 显示城乡类型筛选标签（地级市、县级市、乡镇、村庄），默认显示全部数据，点击类型标签后按类型过滤。
6. IF 访客选择的是海外城乡，THEN THE Template SHALL 将省份名称处显示"海外城乡"，将地级市等类型处显示国家简称。
7. THE PlaceSearchView SHALL 按移动排序优先、添加时间顺序（非倒序）显示搜索结果。

---

### 需求 5：省份介绍弹窗页

**用户故事：** 作为网站访客，我希望在城乡搜索页点击省份名称时，能弹窗查看省份的基本介绍，无需跳转页面。

#### 验收标准

1. THE Template SHALL 在城乡搜索页的省份名称处提供弹窗触发链接。
2. WHEN 访客点击省份名称链接，THE Template SHALL 弹出省份介绍弹窗，显示省份全称和省份介绍（`Region.introduction`）字段内容。
3. THE 省份介绍弹窗 Template SHALL 为极简页面，仅包含省份全称和介绍文字，无其他导航或功能。
4. WHEN 弹窗打开，THE Template SHALL 使背景页面变灰且不可操作，直至弹窗关闭。

---

### 需求 6：详情页视图与模板

**用户故事：** 作为网站访客，我希望每类内容（城乡、资讯、美食、文创、景区/打卡地、交通）都有完整的详情页，展示完整信息并统计阅读量。

#### 验收标准

1. THE PlaceDetailView SHALL 在 `/travelogue/ur{english_code}.shtml` 路径提供城乡详情页视图。
2. THE NewsDetailView SHALL 在 `/news/{slug}.shtml` 路径提供资讯详情页视图。
3. THE FoodDetailView SHALL 在 `/foods/f{offset_id}.shtml` 路径提供美食详情页视图。
4. THE GoodDetailView SHALL 在 `/goods/g{offset_id}.shtml` 路径提供文创详情页视图。
5. THE ScenicSpotDetailView SHALL 在 `/travelogue/scenic{offset_id}.shtml` 路径提供景区/打卡地详情页视图。
6. THE TrafficDetailView SHALL 在 `/travelogue/traffic{offset_id}.shtml` 路径提供交通详情页视图。
7. WHEN 访客访问任意详情页，THE View SHALL 使用 Django `F()` 表达式对 `views` 字段执行原子自增操作，避免并发计数错误。
8. WHEN 城乡详情页加载，THE PlaceDetailView SHALL 按以下固定顺序展示交通数据：机场 > 火车站 > 汽车站 > 水上交通 > 轨道交通 > 地面交通，同类型内按添加时间倒序。
9. WHEN Place.hide_nav 为 True，THE Template SHALL 隐藏城乡详情页的导航栏，仅显示基本资料，并按景区、打卡地、美食、商品、攻略、交通的顺序排列少量关联数据。
10. WHEN 详情页中 `source`（来源）或 `author`（作者）字段为空，THE Template SHALL 不渲染该字段所在的整行 HTML，避免显示空白行。
11. WHEN 详情页中设置了 `redirect_url`，THE View SHALL 返回 HTTP 302 重定向至该 URL，并在新窗口打开。
12. THE Template SHALL 在详情页面包屑的省份名称处提供弹窗链接（国内省份），海外国家名称不添加链接。
13. WHEN 详情页图片启用水印（`image_show_watermark=True`），THE Template SHALL 将图片最大宽度限制为 960px，并在指定位置（`watermark_pos`）叠加 `watermark.png`（208×60 像素，随图片等比缩放）。

---

### 需求 7：首页逻辑修正

**用户故事：** 作为网站访客，我希望首页焦点图和文字推荐展示最新且最相关的内容，而非仅依赖手动标记。

#### 验收标准

1. THE IndexView SHALL 将焦点图数据改为：从所有有图片（`image` 字段非空）的内容中，取最近 3 条（包含置顶内容），而非仅取 `is_home=True` 的前 3 条。
2. THE IndexView SHALL 将文字推荐数据改为：优先显示 `HomeTextRecommend` 中启用的条目，再补充 `is_home=True` 的攻略和资讯，合计最多显示 8 条。
3. WHEN 首页文字推荐数据来源为 `HomeTextRecommend`，THE Template SHALL 不显示日期字段。
4. THE Template SHALL 在首页显示所有启用的搜索关键词（`SearchKeyword.is_active=True`），不限制数量。

---

### 需求 8：前端静态页面

**用户故事：** 作为网站访客，我希望网站提供完整的辅助页面（关于我们、服务协议、404 等），以获得完整的浏览体验。

#### 验收标准

1. THE Template SHALL 提供 404 静态错误页面（`templates/404.html`），页面极简，包含返回首页链接。
2. THE Template SHALL 提供 403 静态错误页面（`templates/403.html`），页面极简，包含返回首页链接。
3. THE Template SHALL 提供登录/注册静态页面（`templates/pages/auth.html`），无后端程序，仅为静态展示页。
4. THE SiteInfoView SHALL 提供关于我们、服务协议、联系我们、合作事宜四个页面，共用同一模板（`templates/pages/site_info.html`），通过 URL 参数区分读取 `SiteInfo` 模型的不同字段。
5. THE FeedbackView SHALL 提供意见建议提交页面（`/feedback/`），包含标题、内容、联系人、联系方式表单，提交后保存至 `Feedback` 模型。
6. WHEN 意见建议表单提交成功，THE FeedbackView SHALL 显示提交成功提示并清空表单。
7. IF 意见建议表单提交时必填字段（标题、内容）为空，THEN THE FeedbackView SHALL 返回表单验证错误提示，不保存数据。

---

### 需求 9：详情页弹窗功能

**用户故事：** 作为网站访客，我希望在详情页能通过弹窗提交报错、揪错和补充材料，而无需跳转页面。

#### 验收标准

1. THE Template SHALL 在所有内容详情页提供"我要报错"弹窗，弹窗表单字段与 `Feedback` 模型一致，提交后保存至后台。
2. THE Template SHALL 在城乡详情页额外提供"补充材料"弹窗，在报错弹窗基础上增加一行说明文字。
3. WHEN 弹窗打开，THE Template SHALL 使背景页面变灰且不可操作，直至弹窗关闭或提交完成。
4. WHEN 弹窗表单提交成功，THE Template SHALL 显示提交成功提示并自动关闭弹窗。

---

### 需求 10：前端显示细节规范

**用户故事：** 作为网站访客，我希望前端页面在字体、图片、链接行为和文字截断等方面保持一致的视觉规范。

#### 验收标准

1. THE Template SHALL 将全站字体统一设置为微软雅黑（`font-family: "Microsoft YaHei", SimSun, sans-serif`），移除 Google Fonts 的 Inter 字体引用。
2. WHEN 访客点击详情页链接，THE Template SHALL 以 `target="_blank"` 在新窗口打开详情页。
3. WHEN 访客点击栏目导航链接，THE Template SHALL 在当前窗口（`target="_self"`）打开栏目主页。
4. WHEN 列表页标题或概述文字超出显示区域，THE Template SHALL 使用 CSS `text-overflow: ellipsis` 截断并显示省略号（`...`），不显示完整文字。
5. THE Template SHALL 在所有列表页图片位置，当内容无焦点图时显示 `noimage.png`（约 225×150 像素），资讯和攻略列表除外（允许无图）。
6. THE Template SHALL 将所有焦点图按等比缩放居中裁剪显示，不拉伸变形。
7. THE NewsListView SHALL 将资讯列表每页显示数量修正为 10 条（当前错误值为 15 条）。
8. THE PlaceListView SHALL 将城乡主页每页显示数量设置为 15 条。
9. THE Template SHALL 在页头二维码图标处实现悬停展开效果：鼠标悬停时显示 `qrcode.png`（112×112 像素）完整二维码，移开后收起。
10. THE Template SHALL 在首页使用 338×98 像素的 Logo 图片，在其他页面使用 170×50 像素的 Logo 图片（或同一图片等比缩放）。
11. WHEN 搜索框提交搜索请求，THE Template SHALL 根据选择的栏目将搜索结果跳转至对应栏目主页，城乡搜索跳转至独立搜索结果页（`/places/search/`）。
12. THE Template SHALL 在底部版权区域，首页使用 `SiteInfo.copyright_home` 字段，其他所有页面使用 `SiteInfo.copyright` 字段，通过独立的模板片段（`_footer.html`）引用。

---

### 需求 11：后台管理增强

**用户故事：** 作为后台管理员，我希望后台提供删除确认、数据移动和表单优化功能，以减少误操作并提升管理效率。

#### 验收标准

1. WHEN 管理员在后台点击删除数据，THE Admin SHALL 弹出确认对话框，要求管理员确认后才执行删除操作。
2. THE Admin SHALL 为城乡（Place）、景区（ScenicSpot）、省份（Region）提供同类别内的排序移动功能（上移/下移）。
3. WHEN 管理员执行移动操作时未选择同一类别的数据，THE Admin SHALL 显示提示"请选择同一类别再进行操作"。
4. THE Admin SHALL 通过 CSS 将后台表单单行文本输入框（`input[type=text]`）的宽度设置为比默认值更宽，提升输入体验。
5. THE Admin SHALL 在城乡编辑表单中显示 `hide_nav`、`place_type`、`is_overseas` 字段，并提供清晰的字段说明。
6. THE Admin SHALL 在交通编辑表单中将 `traffic_type` 显示为下拉选择框，选项为六种交通类型枚举值。

---

### 需求 12：图片处理规范

**用户故事：** 作为内容管理员，我希望系统按照统一规范处理图片上传、显示和水印，保证视觉一致性。

#### 验收标准

1. THE Admin SHALL 将媒体文件上传路径统一设置为 `media/uploads/admin/{YYYYMM}/` 格式，每月自动切换子目录。
2. WHEN 详情页图片宽度超过 960px，THE Template SHALL 将图片最大宽度限制为 960px，高度等比缩放。
3. WHEN 详情页启用水印，THE Template SHALL 在图片上叠加 `watermark.png`（原始尺寸 208×60），水印尺寸随图片等比缩放。
4. WHEN 列表页显示焦点图，THE Template SHALL 不显示水印，仅在详情页显示水印。
5. THE Template SHALL 支持 GIF 动图作为焦点图，不对 GIF 进行静态化处理。

