import {createRouter, createWebHistory } from 'vue-router'
import HomeView from "@/views/HomeView.vue"
import LoginView from "@/views/LoginView.vue"
import RegisterView from "@/views/RegisterView.vue"
import { useAuthStore } from "@/store/auth.store"
import MessangerBlock from '@/components/MessangerBlock.vue'
import PostsBlock from '@/components/PostsBlock.vue'
import ProfileBlock from '@/components/ProfileBlock.vue'
import FriendsBlock from '@/components/FriendsBlock.vue'
import ChatBlock from '@/components/ChatBlock.vue'
import ChatsBlock from '@/components/ChatsBlock.vue'
import SettingsBlock from '@/components/SettingsBlock.vue'
import NotificationBlock from '@/components/NotificationBlock.vue'
import TextBlock from '@/components/TextBlock.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { 
            path: '/', component: HomeView,
            children: [
                {
                    path: 'feed',
                    component: PostsBlock
                },
                {
                    path: 'messanger',
                    component: MessangerBlock,
                    children: [
                      {
                        path: '',
                        components: {
                          ChatsBlock,
                          ChatBlock: TextBlock
                        },
                        props: {
                            ChatsBlock: false,
                            ChatBlock: {
                                message: 'Select any chat'
                            }
                        }
                      },
                      {
                        path: 'chat/:chatId',
                        components: {
                          ChatsBlock,
                          ChatBlock
                        },
                        props: {
                          ChatsBlock: false,
                          ChatBlock: (route) => ({ chatId: Number(route.params.chatId) })
                        }
                      }
                    ]
                },
                {
                    path: 'profile/:userId',
                    component: ProfileBlock,
                    props: (route) => ({ userId: Number(route.params.userId )})
                },
                {
                    path: 'friends',
                    component: FriendsBlock
                },
                {
                    path: 'settings',
                    component: SettingsBlock
                },
                {
                    path: 'notification',
                    component: NotificationBlock
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
});

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