# 旅游信息网站 (Django)

基于 Django 框架开发的旅游资讯展示平台。

## 运行环境要求
*   **Python**: 3.10 / 3.11 / 3.12+ (推荐 3.12)
*   **Django**: 5.2 (LTS 长期支持版)
*   **数据库**: MySQL 8.0.28+ 或 **MariaDB 10.6+** (最佳推荐 MySQL 8.4 LTS)

## 快速启动手册

如果你是在一台新机器上拉取了本代码，请按照以下步骤进行配置：

### 1. 环境准备
```powershell
# 创建并激活虚拟环境
python -m venv venv
.\venv\Scripts\activate

# 安装依赖项
pip install -r requirements.txt
```

### 2. 环境配置
```powershell
copy .env.example .env
```
编辑 `.env` 文件，根据本地环境修改 `DATABASE_URL`。

### 3. 数据库初始化
在确保本地数据库服务已启动并配置正确后：
```powershell
# 执行数据库迁移
python manage.py migrate

# 创建超级管理员
python manage.py createsuperuser
```

### 4. 运行
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
