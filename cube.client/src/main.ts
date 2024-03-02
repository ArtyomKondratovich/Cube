import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { VueQueryPlugin } from 'vue-query'

import router from './helpers/router'
import App from './App.vue'

createApp(App)
    .use(createPinia())
    .use(router)
    .use(VueQueryPlugin)
    .mount('#app')
