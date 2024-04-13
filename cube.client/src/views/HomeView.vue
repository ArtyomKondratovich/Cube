<template>
    <div class="homeView">
        <div class="menu">
            <router-link :to="{ path: `/profile/${user.id}` }">
                <div class="profile">
                    <img v-if="user.avatarBytes != null" :src="userAvatar()">
                    <img v-if="user.avatarBytes == null" src="../assets/Images/Profile/Profile_default.png">
                    <div class="userName"> {{ user?.name }} {{ user?.surname }} </div>
                </div>
            </router-link>      
            <ul>
                <li :class="{ 'active': activeTab == 0 }">
                    <img src="../assets/icons/newsIcon.png">
                    <router-link :to="{ path: '/feed' }">{{ getLabel(0) }}</router-link>
                </li>
                <li :class="{ 'active': activeTab == 1 }">
                    <img src="../assets/icons/messangerIcon.png">
                    <router-link :to="{ path: `/messanger` }">{{ getLabel(1) }}</router-link>
                </li>
                <li :class="{ 'active': activeTab == 2 }">
                    <img src="../assets/icons/notificationIcon.png">
                    <router-link :to="{ path: '/notification' }">{{ getLabel(2) }}</router-link>
                </li>
                <li :class="{ 'active': activeTab == 3 }">
                    <img src="../assets/icons/friendsIcon.png">
                    <router-link :to="{ path: `/friends` }">Friends</router-link>
                </li>
                <li :class="{ 'active': activeTab == 4 }">
                    <img src="../assets/icons/settingsIcon.png">
                    <router-link :to="{ path: '/settings' }">Settings</router-link>
                </li>
            </ul>
        </div>
        <div class="block">
            <router-view></router-view>
        </div>
    </div>
</template>

<script setup lang="ts">
import { inject, computed } from 'vue';
import { type INotificationStore } from "@/store/notification.store";
import type { IUser } from '@/api/types';
import router from '@/helpers/router';

const user = JSON.parse(localStorage.getItem('user') ?? '{}') as IUser;
const activeTab = computed(() => {
    let currentPath = router.currentRoute.value.fullPath;

    if (currentPath.includes('/feed')) {
        return 0;
    }
    else if (currentPath.includes('/messanger')){
        return 1;
    }
    else if (currentPath.includes('/notification')){
        return 2;
    }
    else if (currentPath.includes('/friends')){
        return 3;
    }
    else if (currentPath.includes('/settings')){
        return 4;
    }

    return -1;
})
const store = inject<INotificationStore>('notificationStore') as INotificationStore;
const unreadMessangerNotifications = computed(() => {
    return store.getMessangerNotifications;
});
const unreadFriendNotifications = computed(() => {
    return store.getFriendNotifications;
});
const unreadPostsNotifications = computed(() => {
    return store.getPostsNotifications
});

function userAvatar(): string {
    return 'data:image/jpeg;base64,' + user.avatarBytes;
}

function getLabel(id: number): string {
    let count;
    switch(id){
        case 0:
            count = unreadPostsNotifications.value.length;
            if (count == 0) {
                return 'News';
            }
            return 'News *';
        case 1:
            count = unreadMessangerNotifications.value.length;
            if (count == 0) {
                return 'Messanger';
            }
            return 'Messanger *';
        case 2:
            count = unreadFriendNotifications.value.length;
            if (count == 0) {
                return 'Notifications';
            }
            return 'Notifications *';
    }

    return '';
}
</script>

<style>

.homeView {
    display: flex;
    flex-direction: row;
    justify-content: center;
    height: 100%;
}

.profile {
    border: 1px solid #363738;
    display: flex;
    padding: 10px;
    border-radius: 10px;
    background-color: #222222;
    margin-bottom: 30px;
}

.profile img {
    width: 36px;
    height: 36px;
    border-radius: 18px;
}

.userName {
    margin-left: 10px;
}

.menu {
    display: flex;
    flex-direction: column;
    margin-top: 10px;
    margin-right: 15px;
    width: 15%;
    user-select: none;
    -moz-user-select: none;
    -webkit-user-select: none;
    -ms-user-select: none;
    cursor: default;
}

.menu li:first-child {
    border-top-left-radius: 10px;
    border-top-right-radius: 10px;
}

.menu li:last-child {
    border-bottom-left-radius: 10px;
    border-bottom-right-radius: 10px;
}

.menu li{
    list-style-type: none;
    padding-left: 5px;
}   

.menu ul {
    padding: 0px;
    margin-top: 0px;
    border: 1px solid #363738;
    background-color: #222222;
    border-radius: 10px;
    display: flex;
    flex-direction: column;
}

.menu ul li a {
    display: flex;
    padding: 5px;
    width: 100%;
}

.block {
    display: flex;
    border: 1px solid #363738;
    margin-top: 10px;
    margin-left: 15px;
    border-radius: 10px;
    width: 45%;
    height: 80%;
    background-color: #222222;
}

.active {
    background-color: #222222;
    border-bottom-left-radius: 10px;
    border-top-left-radius: 10px;
}


</style>