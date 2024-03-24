import {createRouter, createWebHistory } from 'vue-router'
import HomeView from "@/views/HomeView.vue"
import LoginView from "@/views/LoginView.vue"
import RegisterView from "@/views/RegisterPage.vue"
import { useAuthStore } from "@/store/auth.store"
import MessangerBlock from '@/components/MessangerBlock.vue'
import PostsBlock from '@/components/PostsBlock.vue'
import ProfileBlock from '@/components/ProfileBlock.vue'
import FriendsBlock from '@/components/FriendsBlock.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { 
            path: '/home', component: HomeView,
            children: [
                {
                    path: 'messanger/:userId',
                    component: MessangerBlock,
                    props: (route) => ({ userId: Number(route.params.userId )})
                },
                {
                    path: 'profile/:userId',
                    component: ProfileBlock,
                    props: (route) => ({ userId: Number(route.params.userId )})
                },
                {
                    path: 'posts',
                    component: PostsBlock
                },
                {
                    path: 'friends/:userId',
                    component: FriendsBlock,
                    props: (route) => ({ userId: Number(route.params.userId )})
                }
            ],
            props: true
        },
        { 
            path: '/login', component: LoginView 
        },
        { 
            path: '/register', component: RegisterView 
        }
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