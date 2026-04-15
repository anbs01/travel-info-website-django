# 旅游信息网站 (Travel Information Website) 🚀

基于 **Django 5.2** 框架构建的高性能旅游资讯展示平台。

## 运行环境要求
*   **Python**: 3.12+ 
*   **Django**: 5.2 (LTS)
*   **数据库**: MySQL 8.4 (LTS) —— 推荐使用 Docker 一键安装

---

## ✨ 核心特性

### 1. 合规路由系统 (SEO & Security)
*   **城乡详情**：`/places/ur/<slug>/` 统一资源定位。
*   **美食/文创详情**：采用 **ID 偏移 (Offset 10000)** 的伪静态路径（如 `/foods/f10001/`），杜绝数据库原始自增 ID 泄露。
*   **资讯/游记详情**：采用 `YYYYMMDDHHmm` 时间戳 Slug，确保 URL 的唯一性与可读性。
*   **全链路对齐**：全站内部链接通过 `get_absolute_url` 封装，支持路由层零成本调整。

### 2. 精致 UI/UX 体验
*   **动态 Logo 系统**：首页大规格显速 (338x98px)，内页简约规格 (170x50px) 自动切换。
*   **交互式二维码**：导航栏图标支持 `hover` 展示官方二维码 (112x112px)。
*   **区域感应面包屑**：在详情页面包屑中点击省份名称，即时唤起详细的区域介绍弹窗。

---

## ⚡ 快速启动手册

如果你是在一台新机器上拉取了本代码，请按照以下步骤进行配置：

> [!IMPORTANT]
> **路径执行约束**：以下所有命令必须在 **项目根目录** (`d:\LQH\Git\travel-info-website-django\`) 下执行。

### 1. 启动数据库 (推荐方式)
确保你的机器上已安装 Docker。
```powershell
# 在根目录运行，一键启动 MySQL 8.4
docker-compose up -d
```

### 2. 环境准备
```powershell
# 创建虚拟环境
python -m venv venv

# 激活环境 (Windows PowerShell)
.\venv\Scripts\activate

# 安装依赖项
pip install -r requirements.txt
```

### 3. 环境配置
```powershell
copy .env.example .env
```
编辑 `.env` 文件，确保 `DATABASE_URL` 与 Docker 容器内的账号密码匹配。

### 4. 数据库初始化
```powershell
# 此时数据库已就绪，直接执行迁移
python manage.py migrate

# 创建超级管理员
python manage.py createsuperuser
```

### 5. 运行开发服务器

> [!TIP]
> **Windows 运行建议**：如果在激活环境下执行报错，请直接使用以下显式路径指令：
> `.\venv\Scripts\python.exe manage.py runserver`

```powershell
python manage.py runserver
```
访问地址：[http://127.0.0.1:8000](http://127.0.0.1:8000)

---

## 🔐 管理后台访问
启动开发服务器后，可通过以下信息登录管理后台进行数据录入：
- **管理地址**: [http://127.0.0.1:8000/admin/](http://127.0.0.1:8000/admin/)
- **默认账号**: `admin`
- **默认密码**: `123456`
- **UI 特性**: 支持 SimpleUI 现代化后台交互。

---

## 📂 目录结构说明
- `apps/`: 业务模块（Food, Good, Place, News, Travelogue）。
- `config/`: 全局配置。
- `docs/`: 项目文档及原始资料。包含 `01-需求文档.md` 及最新的 `04-完工演示文档.md`。
- `static/` & `media/`: 资源文件（含 `qrcode.png` 与 `noimage.png` 占位符）。
- `templates/`: 包含首页与内页分离的 `footer` 等核心模板组件。
