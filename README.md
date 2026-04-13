# 旅游信息网站 (Django)

基于 Django 框架开发的旅游资讯展示平台。

## 运行环境要求
*   **Python**: 3.12+ 
*   **Django**: 5.2 (LTS)
*   **数据库**: MySQL 8.4 (LTS) —— 推荐使用 Docker 一键安装

## 快速启动手册

如果你是在一台新机器上拉取了本代码，请按照以下步骤进行配置：

### 1. 启动数据库 (推荐方式)
确保你的机器上已安装 Docker。
```powershell
# 在根目录运行，一键启动 MySQL 8.4
docker-compose up -d
```

### 2. 环境准备
```powershell
# 创建并激活虚拟环境
python -m venv venv
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
```powershell
python manage.py runserver
```
访问地址：[http://127.0.0.1:8000](http://127.0.0.1:8000)

---

## 目录结构说明
- `apps/`: 业务模块。
- `config/`: 全局配置。
- `docs/`: 项目文档及原始资料。
- `static/` & `media/`: 资源文件。
