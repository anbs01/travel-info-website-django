-- ======================================================
-- 类别热词模拟数据填充脚本
-- ======================================================

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;

-- 清空旧的热词数据
TRUNCATE TABLE tp_hot_words;

-- 1. 首页搜索词 (ShowInHome)
INSERT INTO tp_hot_words (Name, ShowInHome, ShowInPlace, ShowInScenic, ShowInTravelogue, ShowInGuide, ShowInSpecialty, ShowInCuisine, ShowInCreative, ShowInNews, SortOrder, IsHidden, CreatedAt) 
VALUES ('延吉', 1, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, NOW()),
       ('威海', 1, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, NOW()),
       ('故宫', 1, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, NOW()),
       ('泰山', 1, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, NOW()),
       ('美食攻略', 1, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, NOW()),
       ('亲子游', 1, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, NOW());

-- 2. 城乡搜索词 (ShowInPlace)
INSERT INTO tp_hot_words (Name, ShowInHome, ShowInPlace, ShowInScenic, ShowInTravelogue, ShowInGuide, ShowInSpecialty, ShowInCuisine, ShowInCreative, ShowInNews, SortOrder, IsHidden, CreatedAt) 
VALUES ('古镇', 0, 1, 0, 0, 0, 0, 0, 0, 0, 10, 0, NOW()),
       ('乡村', 0, 1, 0, 0, 0, 0, 0, 0, 0, 9, 0, NOW()),
       ('民族村', 0, 1, 0, 0, 0, 0, 0, 0, 0, 8, 0, NOW()),
       ('红色基地', 0, 1, 0, 0, 0, 0, 0, 0, 0, 7, 0, NOW()),
       ('康养小镇', 0, 1, 0, 0, 0, 0, 0, 0, 0, 6, 0, NOW());

-- 3. 打卡地类别 (ShowInScenic)
INSERT INTO tp_hot_words (Name, ShowInHome, ShowInPlace, ShowInScenic, ShowInTravelogue, ShowInGuide, ShowInSpecialty, ShowInCuisine, ShowInCreative, ShowInNews, SortOrder, IsHidden, CreatedAt) 
VALUES ('博物馆', 0, 0, 1, 0, 0, 0, 0, 0, 0, 10, 0, NOW()),
       ('名胜古迹', 0, 0, 1, 0, 0, 0, 0, 0, 0, 9, 0, NOW()),
       ('自然风光', 0, 0, 1, 0, 0, 0, 0, 0, 0, 8, 0, NOW()),
       ('网红打卡', 0, 0, 1, 0, 0, 0, 0, 0, 0, 7, 0, NOW()),
       ('游乐园', 0, 0, 1, 0, 0, 0, 0, 0, 0, 6, 0, NOW());

-- 4. 纪行类别 (ShowInTravelogue)
INSERT INTO tp_hot_words (Name, ShowInHome, ShowInPlace, ShowInScenic, ShowInTravelogue, ShowInGuide, ShowInSpecialty, ShowInCuisine, ShowInCreative, ShowInNews, SortOrder, IsHidden, CreatedAt) 
VALUES ('深度游', 0, 0, 0, 1, 0, 0, 0, 0, 0, 10, 0, NOW()),
       ('周末游', 0, 0, 0, 1, 0, 0, 0, 0, 0, 9, 0, NOW()),
       ('毕业旅行', 0, 0, 0, 1, 0, 0, 0, 0, 0, 8, 0, NOW()),
       ('自驾游', 0, 0, 0, 1, 0, 0, 0, 0, 0, 7, 0, NOW()),
       ('徒步', 0, 0, 0, 1, 0, 0, 0, 0, 0, 6, 0, NOW());

-- 5. 攻略类别 (ShowInGuide)
INSERT INTO tp_hot_words (Name, ShowInHome, ShowInPlace, ShowInScenic, ShowInTravelogue, ShowInGuide, ShowInSpecialty, ShowInCuisine, ShowInCreative, ShowInNews, SortOrder, IsHidden, CreatedAt) 
VALUES ('交通指南', 0, 0, 0, 0, 1, 0, 0, 0, 0, 10, 0, NOW()),
       ('住宿推荐', 0, 0, 0, 0, 1, 0, 0, 0, 0, 9, 0, NOW()),
       ('省钱攻略', 0, 0, 0, 0, 1, 0, 0, 0, 0, 8, 0, NOW()),
       ('摄影建议', 0, 0, 0, 0, 1, 0, 0, 0, 0, 7, 0, NOW()),
       ('装备清单', 0, 0, 0, 0, 1, 0, 0, 0, 0, 6, 0, NOW());

-- 6. 特产类别 (ShowInSpecialty)
INSERT INTO tp_hot_words (Name, ShowInHome, ShowInPlace, ShowInScenic, ShowInTravelogue, ShowInGuide, ShowInSpecialty, ShowInCuisine, ShowInCreative, ShowInNews, SortOrder, IsHidden, CreatedAt) 
VALUES ('土特产', 0, 0, 0, 0, 0, 1, 0, 0, 0, 10, 0, NOW()),
       ('传统工艺', 0, 0, 0, 0, 0, 1, 0, 0, 0, 9, 0, NOW()),
       ('时令水果', 0, 0, 0, 0, 0, 1, 0, 0, 0, 8, 0, NOW()),
       ('药膳食材', 0, 0, 0, 0, 0, 1, 0, 0, 0, 7, 0, NOW());

-- 7. 美食菜系 (ShowInCuisine)
INSERT INTO tp_hot_words (Name, ShowInHome, ShowInPlace, ShowInScenic, ShowInTravelogue, ShowInGuide, ShowInSpecialty, ShowInCuisine, ShowInCreative, ShowInNews, SortOrder, IsHidden, CreatedAt) 
VALUES ('鲁菜', 0, 0, 0, 0, 0, 0, 1, 0, 0, 10, 0, NOW()),
       ('川菜', 0, 0, 0, 0, 0, 0, 1, 0, 0, 9, 0, NOW()),
       ('粤菜', 0, 0, 0, 0, 0, 0, 1, 0, 0, 8, 0, NOW()),
       ('东北菜', 0, 0, 0, 0, 0, 0, 1, 0, 0, 7, 0, NOW()),
       ('延边特色', 0, 0, 0, 0, 0, 0, 1, 0, 0, 6, 0, NOW()),
       ('威海海鲜', 0, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, NOW());

-- 8. 文创类别 (ShowInCreative)
INSERT INTO tp_hot_words (Name, ShowInHome, ShowInPlace, ShowInScenic, ShowInTravelogue, ShowInGuide, ShowInSpecialty, ShowInCuisine, ShowInCreative, ShowInNews, SortOrder, IsHidden, CreatedAt) 
VALUES ('冰箱贴', 0, 0, 0, 0, 0, 0, 0, 1, 0, 10, 0, NOW()),
       ('明信片', 0, 0, 0, 0, 0, 0, 0, 1, 0, 9, 0, NOW()),
       ('联名产品', 0, 0, 0, 0, 0, 0, 0, 1, 0, 8, 0, NOW()),
       ('手办模型', 0, 0, 0, 0, 0, 0, 0, 1, 0, 7, 0, NOW());

-- 9. 资讯类别 (ShowInNews)
INSERT INTO tp_hot_words (Name, ShowInHome, ShowInPlace, ShowInScenic, ShowInTravelogue, ShowInGuide, ShowInSpecialty, ShowInCuisine, ShowInCreative, ShowInNews, SortOrder, IsHidden, CreatedAt) 
VALUES ('行业新闻', 0, 0, 0, 0, 0, 0, 0, 0, 1, 10, 0, NOW()),
       ('优惠政策', 0, 0, 0, 0, 0, 0, 0, 0, 1, 9, 0, NOW()),
       ('交通动态', 0, 0, 0, 0, 0, 0, 0, 0, 1, 8, 0, NOW()),
       ('活动预告', 0, 0, 0, 0, 0, 0, 0, 0, 1, 7, 0, NOW());
