-- ======================================================
-- 首页推荐模拟数据填充脚本
-- ======================================================

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;

-- 清空旧数据
TRUNCATE TABLE tp_recommendations;

-- 插入模拟数据
INSERT INTO tp_recommendations (Title, ImageUrl, LinkUrl, ClickCount, IsPinned, CreatedAt, EndDate)
VALUES 
('爱在烟台，难以离开', 'https://images.unsplash.com/photo-1540660290370-8af90a454417?w=400', 'https://example.com/yantai', 355, 1, '2025-01-12 18:36:00', NULL),
('京津古镇，杨柳西青。', 'https://images.unsplash.com/photo-1527685238219-c81b3dd33021?w=400', 'https://example.com/town', 355, 0, '2025-01-12 18:36:00', '2027-05-12 18:36:00'),
('最美漓江水，源泉在兴安。', 'https://images.unsplash.com/photo-1508804185872-d7badad00f7d?w=400', 'https://example.com/lijiang', 2, 0, '2025-01-12 18:36:00', '2025-05-12 18:36:00'),
('这是文字推荐，前边就不会有图片', NULL, 'https://example.com/text', 555, 0, '2025-01-12 18:36:00', NULL);
