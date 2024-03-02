import {createRouter, createWebHistory } from 'vue-router'
import Home from "@/views/HomePage.vue"
import Login from "@/views/LoginPage.vue"
import Register from "@/views/RegisterPage.vue"
import { useAuthStore } from '@/stores/authStore'

const router = createRouter({
    history: createWebHistory('/'),
    routes: [
        { path: '/', component: Home },
        { path: '/login', component: Login },
        { path: '/register', component: Register },
    ]
})

router.beforeEach((from, to, next) => {
    const publicPages = ['/login', '/register'];
    const authRequired = !publicPages.includes(to.path);
    const store = useAuthStore()
    
    if (authRequired && store.isLoggedIn)
    {
        next('/login');
    }

    next();
});

export default router;