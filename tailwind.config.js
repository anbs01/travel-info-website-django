/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./templates/**/*.html",
    "./templates/pages/**/*.html",
    "./apps/**/templates/**/*.html",
    "./static/**/*.js",
  ],
  theme: {
    extend: {
      colors: {
        'travel': {
          'brown': '#A67C52',    /* 主强调色 - 按钮/标题 */
          'brown-light': '#B28F5F', /* 悬停/次级色 */
          'dark': '#333333',     /* 主文字色 */
          'gray': '#666666',     /* 次要文字/元数据 */
          'light-bg': '#F9F9F9', /* 灰色背景块 */
        }
      },
      container: {
        center: true,
        padding: '1rem',
        screens: {
          sm: '640px',
          md: '768px',
          lg: '1024px',
          xl: '1200px', /* 对齐设计稿常用宽度 */
        },
      },
      fontFamily: {
        'sans': ['"Inter"', '"PingFang SC"', '"Microsoft YaHei"', 'sans-serif'],
      }
    },
  },
  plugins: [],
}
