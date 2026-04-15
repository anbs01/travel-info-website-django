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
        }
      },
      spacing: {
        '15px': '15px',
        '112px': '112px',
        '252px': '252px',
      },
      maxWidth: {
        'design': '1202px',
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
          xl: '1202px', 
        },
      },
      fontFamily: {
        'sans': ['"Inter"', '"PingFang SC"', '"Microsoft YaHei"', 'sans-serif'],
      }
    },
  },
  plugins: [],
}
