import {createRouter, createWebHistory } from 'vue-router'
import Home from "@/views/HomePage.vue"
import Login from "@/views/LoginPage.vue"
import Register from "@/views/RegisterPage.vue"
import { useAuthStore } from "@/store/auth.store"

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/home', component: Home },
        { path: '/login', component: Login },
        { path: '/register', component: Register },
    ]
})

router.beforeEach((to,from, next) => {
    const publicPages = ['/login', '/register'];
    const authRequired = !publicPages.includes(to.path);
    const store = useAuthStore();
    const loggedIn = store.isLoggedIn;

    if (authRequired && !loggedIn)
    {
        next('/login');
    }
    else {
        next();
    }
});

export default router;