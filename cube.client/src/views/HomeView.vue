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
                <li @click="selectTab(0)" :class="{ 'active': activeTab == 0 }">
                    <img src="../assets/icons/newsIcon.png">
                    <router-link :to="{ path: '/feed' }">{{ getLabel(0) }}</router-link>
                </li>
                <li @click="selectTab(1)" :class="{ 'active': activeTab == 1 }">
                    <img src="../assets/icons/messangerIcon.png">
                    <router-link :to="{ path: `/messanger` }">{{ getLabel(1) }}</router-link>
                </li>
                <li @click="selectTab(2)" :class="{ 'active': activeTab == 2 }">
                    <img src="../assets/icons/notificationIcon.png">
                    <router-link :to="{ path: '/feed' }">{{ getLabel(2) }}</router-link>
                </li>
                <li @click="selectTab(3)" :class="{ 'active': activeTab == 3 }">
                    <img src="../assets/icons/friendsIcon.png">
                    <router-link :to="{ path: `/friends` }">Friends</router-link>
                </li>
                <li @click="selectTab(4)" :class="{ 'active': activeTab == 4 }">
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
import { ref } from 'vue';
import { useNotificationStore } from "@/store/notification.store";
import type { IUser } from '@/api/types';

const user = JSON.parse(localStorage.getItem('user') ?? '{}') as IUser;
const activeTab = ref(0);
const notificationstore = ref(useNotificationStore());

function selectTab(id: number) {
    activeTab.value = id;
}

function userAvatar(): string {
    return 'data:image/jpeg;base64,' + user.avatarBytes;
}

function getLabel(id: number): string {
    let count;
    switch(id){
        case 0:
            count = notificationstore.value.getPostsNotifications;
            if (count == 0) {
                return 'News';
            }
            return 'News ' + `${count}`;
        case 1:
            count = notificationstore.value.getMessangerNotifications;
            if (count == 0) {
                return 'Messanger';
            }
            return 'Messanger ' + `${count}`;
        case 2:
            count = notificationstore.value.getFriendNotifications;
            if (count == 0) {
                return 'Notifications';
            }
            return 'Notifications ' + `${count}`;
    }

    return '';
}

setInterval(notificationstore.value.updateNotificationData, 10000);

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