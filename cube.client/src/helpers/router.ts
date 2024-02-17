import {createRouter, createWebHistory } from 'vue-router'
import Home from "@/views/HomePage.vue"
import Login from "@/views/LoginPage.vue"
import Register from "@/views/RegisterPage.vue"

const router = createRouter({
    history: createWebHistory(''),
    routes: [
        { path: '/', component: Home },
        { path: '/login', component: Login },
        { path: '/register', component: Register },
    ]
})

export default router;