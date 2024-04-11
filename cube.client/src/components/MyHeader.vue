<template>
        <div class="nav-blocks">
            <div class="logo">
                <router-link to="/feed">
                    <img src="../assets/icons/cubeIcon.png">
                </router-link>
            </div>
            <div class="links">
                <ul>
                    <li v-if="!authStore?.isLoggedIn" id="signin">
                        <router-link to="/login">Login</router-link>
                    </li>
                    <li id="signup">
                        <router-link to="/register">SignUp</router-link>
                    </li>
                    <li id="logout">
                        <a @click="logout">Logout</a>
                    </li>
                </ul>
            </div>
        </div>
</template>

<script setup lang="ts">
import { inject } from 'vue';
import { type IAuthStore } from '../store/auth.store';
import type { INotificationStore } from '@/store/notification.store';
const authStore = inject<IAuthStore>('authStore');
const notificationStore = inject<INotificationStore>('notificationStore');

function logout() : void {
    if (authStore && notificationStore){
        authStore.logout();
        notificationStore.logout();
    }
}
</script>
<style>
    .nav-blocks {
        display: flex;
        justify-content: space-between;
        width: 62%;
    }

    .links ul{
        display: flex;
        padding: 0;

    }
    .links li{
        list-style-type: none;
        margin-left: 5px;
        margin-right: 5px;
    }
    a{
        text-decoration: none;
        cursor: pointer;
    }
    a, a:visited, a:hover, a:active {
        color: white;
    }
</style>