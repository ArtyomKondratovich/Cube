import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import Welcome from "@/views/Welcome-item.vue"
import Login from "@/views/Login-item.vue"
import Register from "@/views/Register-item.vue"

const routes: Array<RouteRecordRaw> = [
    { path: '', component: Welcome },
    { path: '/login', component: Login },
    { path: '/register', component: Register },
]

const router = createRouter({
    history: createWebHistory("http://localhost:5173/"),
    routes
})

export default router   