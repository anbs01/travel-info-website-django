# 高级前端开发规范 (Frontend Expert Standards)

## 1. 还原准则
*   **1:1 精准还原**：严格按照设计稿（PSD/高清图）的尺寸、间距、比例进行还原。数据必须来源于设计稿数字化解析。
*   **语义化 HTML5**：优先使用 `<header>`, `<nav>`, `<main>`, `<section>`, `<footer>`, `<article>` 等语义化标签。

## 2. 布局与样式约束
*   **Tailwind 优先**：所有样式必须通过 Tailwind CSS 实现，避免零散的写 Style 标签。
*   **布局去绝对化**：**严禁**使用 `position: absolute` 来进行大块布局定位（除非是覆盖式的悬浮 Icon 或 Tooltip）。
*   **现代布局引擎**：必须使用 **Flexbox** 或 **CSS Grid** 进行响应式布局，通过 `gap`、`margin`、`padding` 还原设计稿间距。
*   **全局设计变量**：从设计稿提取主色调、辅助色，并在 `tailwind.config.js` 或 CSS 变量中统一定义。

## 3. 自适应要求
*   所有页面必须具备自适应能力，确保在 375px (Mobile) 到 1920px (Desktop) 范围内布局不崩溃、不溢出。
