import { createApp } from 'vue'
import './assets/css/global.scss'
import App from './App.vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import router from './router/index'
import store from './vuex/index'

createApp(App)
    .use(ElementPlus)
    .use(router)
    .use(store)
    .mount('#app')
