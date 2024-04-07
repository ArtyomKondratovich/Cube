import { createApp } from 'vue'
import { createPinia } from 'pinia'
import Vue3Toasity, { toast, type ToastContainerOptions } from 'vue3-toastify';
import router from './helpers/router'
import App from './App.vue'
import { useNotificationStore } from './store/notification.store';
import { useAuthStore } from './store/auth.store';

createApp(App)
    .use(createPinia())
    .provide('notificationStore', useNotificationStore())
    .provide('authStore', useAuthStore())
    .use(router)
    .use(Vue3Toasity, {
        role: 'custome-role',
        limit: 1,
        autoClose: 1500,
        position: toast.POSITION.TOP_RIGHT
    } as ToastContainerOptions)
    .mount('#app')