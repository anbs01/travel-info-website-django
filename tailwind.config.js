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
        '16px': '16px',
        '30px': '30px',
        '40px': '40px',
        '43px': '43px',
        '44px': '44px',
        '50px': '50px',
        '64px': '64px',
        '100px': '100px',
        '110px': '110px',
        '112px': '112px',
        '130px': '130px',
        '250px': '250px',
        '252px': '252px',
        '320px': '320px',
        '324px': '324px',
        '340px': '340px',
        '384px': '384px',
        '480px': '480px',
        '484px': '484px',
        '720px': '720px',
        '1200px': '1200px',
      },
      maxWidth: {
        'design': '1200px',
      },
      container: {
        center: true,
        padding: {
          DEFAULT: '1rem',
          xl: '0',
        },
        screens: {
          sm: '640px',
          md: '768px',
          lg: '1024px',
          xl: '1200px', 
        },
      },
      fontFamily: {
        'sans': ['"SimSun"', '"宋体"', '"Microsoft YaHei"', '"微软雅黑"', 'sans-serif'],
        'serif': ['"SimSun"', '"宋体"', 'serif'],
      }
    },
  },
  plugins: [],
}
