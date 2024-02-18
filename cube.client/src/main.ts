import { createApp } from 'vue'
import router from './helpers/router'
import store from "@/stores"

import App from './App.vue'

createApp(App)
    .use(router)
    .use(store)
    .mount('#app')
