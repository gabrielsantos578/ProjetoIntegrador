import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import reactRefresh from '@vitejs/plugin-react-refresh'

export default defineConfig({
  plugins: [react(), reactRefresh()],
  server: {
    host: '0.0.0.0', // Permite acesso de qualquer endereço IP
    port: 5173, // A porta em que o servidor estará escutando
    proxy: {
      '/api': {
        target: 'https://192.168.0.106:5173', // Atualize com o IP e porta do seu backend
        changeOrigin: true,
        secure: true,  // TRUE para HTTPS
        rewrite: (path) => path.replace(/^\/api/, '')
      }
    }
  }
})
