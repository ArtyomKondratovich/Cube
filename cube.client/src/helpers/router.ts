import {createRouter, createWebHistory } from 'vue-router'
import Home from "@/views/HomePage.vue"
import Login from "@/views/LoginView.vue"
import Register from "@/views/RegisterPage.vue"
import { useAuthStore } from "@/store/auth.store"
import Messages from '@/components/Messages.vue'
import Posts from '@/components/Posts.vue'
import Chat from '@/components/Chat.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/home', component: Home,
        children: [
            {
                path: 'messages',
                component: Messages
            },
            {
                path: 'posts',
                component: Posts
            },
            {
                path: 'chat/:id',
                component: Chat,
                
            }
        ]},
        { path: '/login', components: {
            default: Login
        }},
        { path: '/register', components: {
            default: Register
        }}
    ]
})

router.beforeEach((to, from, next) => {
    const publicPages = ['/login', '/register'];
    const authRequired = !publicPages.includes(to.path);
    const store = useAuthStore();
    const loggedIn = store.isLoggedIn;

    if (authRequired && !loggedIn)
    {
        next('/login');
    }
    else{
        next();
    }
});

export default router;