SET NAMES utf8mb4;
USE TravelPortal_DB;

-- 1. 地理表 tp_geo (扩展为全功能 BaseContent)
ALTER TABLE tp_geo 
ADD COLUMN FullTitle varchar(200) DEFAULT NULL AFTER Title,
ADD COLUMN SubTitle varchar(200) DEFAULT NULL AFTER FullTitle,
ADD COLUMN Summary text DEFAULT NULL AFTER SubTitle,
ADD COLUMN Content longtext DEFAULT NULL AFTER Summary,
ADD COLUMN MainImage varchar(500) DEFAULT NULL AFTER Content,
ADD COLUMN PublishDate datetime DEFAULT NULL AFTER MainImage,
ADD COLUMN Source varchar(100) DEFAULT NULL AFTER PublishDate,
ADD COLUMN Author varchar(100) DEFAULT NULL AFTER Source,
ADD COLUMN OriginUrl varchar(500) DEFAULT NULL AFTER Author,
ADD COLUMN RedirectUrl varchar(500) DEFAULT NULL AFTER OriginUrl,
ADD COLUMN GeoCode varchar(50) DEFAULT NULL AFTER RedirectUrl,
ADD COLUMN GeoTag varchar(100) DEFAULT NULL AFTER GeoCode,
ADD COLUMN EnglishName varchar(200) DEFAULT NULL AFTER GeoTag,
ADD COLUMN JurisdictionLayers varchar(200) DEFAULT NULL AFTER EnglishName,
ADD COLUMN ShowWatermark tinyint(1) NOT NULL DEFAULT 0,
ADD COLUMN WatermarkPos varchar(20) DEFAULT NULL,
ADD COLUMN ShowNavStrip tinyint(1) NOT NULL DEFAULT 0,
ADD COLUMN Views int NOT NULL DEFAULT 0,
ADD COLUMN IsSticky tinyint(1) NOT NULL DEFAULT 0,
ADD COLUMN StickyAt datetime DEFAULT NULL,
ADD COLUMN IsHome tinyint(1) NOT NULL DEFAULT 0,
ADD COLUMN UpdatedAt datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;

-- 2. 打卡点 tp_scenic_spots
ALTER TABLE tp_scenic_spots
ADD COLUMN SubTitle varchar(200) DEFAULT NULL AFTER FullTitle,
ADD COLUMN Content longtext DEFAULT NULL AFTER Summary,
ADD COLUMN FameLevel varchar(50) DEFAULT NULL,
ADD COLUMN ScenicGrade varchar(50) DEFAULT NULL,
ADD COLUMN OriginUrl varchar(500) DEFAULT NULL,
ADD COLUMN ShowNavStrip tinyint(1) NOT NULL DEFAULT 0,
DROP COLUMN SpotType;

-- 3. 美食特产 tp_foods
ALTER TABLE tp_foods
CHANGE COLUMN Category ProductType varchar(50) NOT NULL DEFAULT '美食',
CHANGE COLUMN HeritageLevel NonLegacyLevel varchar(100) DEFAULT NULL,
ADD COLUMN SubTitle varchar(200) DEFAULT NULL AFTER FullTitle,
ADD COLUMN OriginUrl varchar(500) DEFAULT NULL,
ADD COLUMN ShowNavStrip tinyint(1) NOT NULL DEFAULT 0,
DROP COLUMN Cuisine,
DROP COLUMN SpecialtyCategory;

-- 4. 纪行攻略 tp_travelogues
ALTER TABLE tp_travelogues
CHANGE COLUMN Category Classification varchar(50) NOT NULL DEFAULT '纪行',
ADD COLUMN SubTitle varchar(200) DEFAULT NULL AFTER FullTitle,
ADD COLUMN OriginUrl varchar(500) DEFAULT NULL,
ADD COLUMN ShowNavStrip tinyint(1) NOT NULL DEFAULT 0,
DROP COLUMN Slug,
DROP COLUMN Tags;

-- 5. 非遗文创 tp_creative_products
ALTER TABLE tp_creative_products
ADD COLUMN SubTitle varchar(200) DEFAULT NULL AFTER FullTitle,
ADD COLUMN OriginUrl varchar(500) DEFAULT NULL,
ADD COLUMN ShowNavStrip tinyint(1) NOT NULL DEFAULT 0,
ADD COLUMN NonLegacyLevel varchar(100) DEFAULT NULL,
DROP COLUMN Category;

-- 6. 行业资讯 tp_news
ALTER TABLE tp_news
CHANGE COLUMN Category NewsCategory varchar(50) NOT NULL DEFAULT 'news',
ADD COLUMN OriginUrl varchar(500) DEFAULT NULL,
ADD COLUMN ShowNavStrip tinyint(1) NOT NULL DEFAULT 0,
DROP COLUMN PreTitle,
DROP COLUMN PlaceId,
DROP COLUMN Excerpt;

-- 7. 新增中间表
CREATE TABLE IF NOT EXISTS tp_content_categories (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ContentId int NOT NULL,
    CategoryId int NOT NULL,
    Module varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
