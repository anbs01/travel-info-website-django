# 旅游信息网站 (Django)

基于 Django 框架开发的旅游资讯展示平台。

## 快速启动手册

如果你是在一台新机器上拉取了本代码，请按照以下步骤进行配置：

### 1. 环境准备
确保你的机器已安装 Python 3.10+。

```powershell
# 创建并激活虚拟环境
python -m venv venv
.\venv\Scripts\activate

# 安装依赖项
pip install -r requirements.txt
```

### 2. 环境配置
本项目使用 `.env` 文件管理配置。请根据模板创建：

```powershell
copy .env.example .env
```
然后编辑 `.env` 文件，根据你的本地环境修改数据库连接字符串（`DATABASE_URL`）和其他关键变量。

### 3. 数据库初始化
在确保本地数据库服务已启动并配置正确后，运行以下命令：

```powershell
# 执行数据库迁移（创建表结构）
python manage.py migrate

# 创建管理员账号
python manage.py createsuperuser
```

### 4. 运行开发服务器
```powershell
python manage.py runserver
```
访问 [http://127.0.0.1:8000](http://127.0.0.1:8000) 查看效果。

---

## 目录结构说明
- `apps/`: 存放业务逻辑模块。
- `config/`: 项目全局配置信息。
- `docs/`: 存放项目详细文档及原始资料。
- `static/` & `media/`: 存放静态资源和用户上传文件。
