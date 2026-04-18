/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Pages/**/*.cshtml",
    "./Views/**/*.cshtml",
    "./wwwroot/**/*.html",
    "./Components/**/*.cshtml"
  ],
  theme: {
    extend: {
      colors: {
        'travel': {
          'brown': '#A67C52',    
          'brown-light': '#B28F5F', 
          'dark': '#333333',     
          'gray': '#666666',     
          'light-bg': '#F9F9F9', 
        },
        'design': {
          'black': '#000000',
          'gray-text': '#666666',
          'primary': '#A67C52',
          'bg-white': '#FFFFFF',
        }
      },
      spacing: {
        '15px': '15px',
        '30px': '30px',
        '1200px': '1200px',
      },
      maxWidth: {
        'design': '1200px',
      },
      fontFamily: {
        'sans': ['"Microsoft YaHei"', '"微软雅黑"', 'system-ui', 'sans-serif'],
      }
    },
  },
  plugins: [],
}
