import { createRouter, createWebHistory } from 'vue-router';

import Welcome from "@/views/WelcomePage.vue"
import Login from "@/views/LoginPage.vue"
import Register from "@/views/RegisterPage.vue"

export const router = createRouter({
    history: createWebHistory(),
    base: "http://localhost:5173",
    routes: [
        { path: '/', component: Welcome },
        { path: '/login', component: Login },
        { path: '/register', component: Register }
    ]
});

router.beforeEach((to, from, next) => {
    // redirect to login page if not logged in and trying to access a restricted page
    const publicPages = ['/login', '/register'];
    const authRequired = !publicPages.includes(to.path);
    const loggedIn = localStorage.getItem('user');

    if (authRequired && !loggedIn) {
        return next('/login');
    }

    next();
})