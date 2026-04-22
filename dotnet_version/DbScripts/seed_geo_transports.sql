SET NAMES utf8mb4;

-- 清理旧数据
DELETE FROM tp_transports;
DELETE FROM tp_geo;

-- 1. 顶级：国家 (Level 1)
INSERT INTO tp_geo (Id, Title, Level, ParentId, AncestorPath, Nature, Slug, SortOrder, IsHidden, CreatedAt) VALUES
(1, '中国', 1, 0, ',1,', 'Domestic', 'china', 1, 0, NOW());

-- 2. 省份 (Level 2)
INSERT INTO tp_geo (Id, Title, Level, ParentId, AncestorPath, Nature, Slug, SortOrder, IsHidden, CreatedAt) VALUES
(2, '山东省', 2, 1, ',1,2,', 'Domestic', 'shandong', 1, 0, NOW()),
(3, '广东省', 2, 1, ',1,3,', 'Domestic', 'guangdong', 2, 0, NOW()),
(4, '浙江省', 2, 1, ',1,4,', 'Domestic', 'zhejiang', 3, 0, NOW());

-- 3. 城市 (Level 3)
INSERT INTO tp_geo (Id, Title, Level, ParentId, AncestorPath, Nature, Slug, SortOrder, IsHidden, CreatedAt) VALUES
-- 山东城市
(5, '烟台市', 3, 2, ',1,2,5,', 'Domestic', 'yantai', 1, 0, NOW()),
(6, '济南市', 3, 2, ',1,2,6,', 'Domestic', 'jinan', 2, 0, NOW()),
(7, '青岛市', 3, 2, ',1,2,7,', 'Domestic', 'qingdao', 3, 0, NOW()),
-- 广东城市
(8, '广州市', 3, 3, ',1,3,8,', 'Domestic', 'guangzhou', 1, 0, NOW()),
(9, '深圳市', 3, 3, ',1,3,9,', 'Domestic', 'shenzhen', 2, 0, NOW());

-- 4. 区县 (Level 4)
INSERT INTO tp_geo (Id, Title, Level, ParentId, AncestorPath, Nature, Slug, SortOrder, IsHidden, CreatedAt) VALUES
-- 烟台区县
(10, '芝罘区', 4, 5, ',1,2,5,10,', 'Domestic', 'zhifu', 1, 0, NOW()),
(11, '莱山区', 4, 5, ',1,2,5,11,', 'Domestic', 'laishan', 2, 0, NOW()),
(12, '蓬莱区', 4, 5, ',1,2,5,12,', 'Domestic', 'penglai', 3, 0, NOW()),
(13, '海阳市', 4, 5, ',1,2,5,13,', 'Domestic', 'haiyang', 4, 0, NOW()),
-- 广州区县
(14, '天河区', 4, 8, ',1,3,8,14,', 'Domestic', 'tianhe', 1, 0, NOW()),
(15, '越秀区', 4, 8, ',1,3,8,15,', 'Domestic', 'yuexiu', 2, 0, NOW());

-- 5. 乡镇 (Level 5)
INSERT INTO tp_geo (Id, Title, Level, ParentId, AncestorPath, Nature, Slug, SortOrder, IsHidden, CreatedAt) VALUES
-- 蓬莱乡镇
(16, '登州街道', 5, 12, ',1,2,5,12,16,', 'Domestic', 'dengzhou', 1, 0, NOW()),
(17, '紫荆山街道', 5, 12, ',1,2,5,12,17,', 'Domestic', 'zijing', 2, 0, NOW());

-- 6. 村庄 (Level 6)
INSERT INTO tp_geo (Id, Title, Level, ParentId, AncestorPath, Nature, Slug, SortOrder, IsHidden, CreatedAt) VALUES
(18, '水城村', 6, 16, ',1,2,5,12,16,18,', 'Domestic', 'shuicheng', 1, 0, NOW());

-- 7. 境外城镇 (Overseas)
INSERT INTO tp_geo (Id, Title, Level, ParentId, AncestorPath, Nature, Slug, SortOrder, IsHidden, CreatedAt) VALUES
(19, '日本', 1, 0, ',19,', 'Overseas', 'japan', 10, 0, NOW()),
(20, '东京', 3, 19, ',19,20,', 'Overseas', 'tokyo', 1, 0, NOW()),
(21, '新加坡', 1, 0, ',21,', 'Overseas', 'singapore', 11, 0, NOW());

-- 8. 交通点数据 (Transports)
INSERT INTO tp_transports (Title, GeoId, TransportType, Address, ContactPhone, ShowWatermark, Views, IsSticky, IsHome, IsHidden, CreatedAt, UpdatedAt) VALUES
-- 烟台
('烟台蓬莱国际机场', 5, 'Airport', '烟台市蓬莱区潮水镇', '0535-5139999', 0, 0, 0, 0, 0, NOW(), NOW()),
('烟台站', 5, 'Railway', '烟台市芝罘区芝罘屯路2号', '0535-95105105', 0, 0, 0, 0, 0, NOW(), NOW()),
('烟台南站', 5, 'Railway', '烟台市莱山区山海南路', '0535-95105105', 0, 0, 0, 0, 0, NOW(), NOW()),
('烟台港客运站', 5, 'Pier', '烟台市芝罘区北马路155号', '0535-6622236', 0, 0, 0, 0, 0, NOW(), NOW()),
-- 济南
('济南遥墙国际机场', 6, 'Airport', '济南市历城区遥墙镇', '0531-96888', 0, 0, 0, 0, 0, NOW(), NOW()),
('济南西站', 6, 'Railway', '济南市槐荫区齐鲁大道', '0531-12306', 0, 0, 0, 0, 0, NOW(), NOW()),
-- 青岛
('青岛胶东国际机场', 7, 'Airport', '青岛市胶州市胶东街道', '0532-96567', 0, 0, 0, 0, 0, NOW(), NOW()),
('青岛站', 7, 'Railway', '青岛市市南区泰安路2号', '0532-12306', 0, 0, 0, 0, 0, NOW(), NOW()),
-- 广州
('广州白云国际机场', 8, 'Airport', '广州市白云区人和镇', '020-36066999', 0, 0, 0, 0, 0, NOW(), NOW()),
('广州南站', 8, 'Railway', '广州市番禺区石壁街道', '020-12306', 0, 0, 0, 0, 0, NOW(), NOW()),
-- 深圳
('深圳宝安国际机场', 9, 'Airport', '深圳市宝安区福永街道', '0755-23456789', 0, 0, 0, 0, 0, NOW(), NOW()),
('深圳北站', 9, 'Railway', '深圳市龙华区民治街道', '0755-12306', 0, 0, 0, 0, 0, NOW(), NOW());
