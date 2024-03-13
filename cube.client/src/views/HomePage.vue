<template>
    <div class="main">
        <div class="menu">
            <ul>
                <li>
                <div class="profile">
                    Profile
                </div>
                </li>
                <li @click="selectTab(0)" :class="{ 'active': activeTab == 0 }">
                    <router-link :to="{ path: '/home/posts' }">Home</router-link>
                </li>
                <li @click="selectTab(1)" :class="{ 'active': activeTab == 1 }">
                    <router-link :to="{ path: '/home/messages' }">Messages</router-link>
                </li>
                <li @click="selectTab(2)" :class="{ 'active': activeTab == 2 }">
                    <div>Notifications</div>
                </li>
                <li @click="selectTab(3)" :class="{ 'active': activeTab == 3 }">
                    <div>Friends</div>
                </li>
                <li @click="selectTab(4)" :class="{ 'active': activeTab == 4 }">
                    <div>Settings</div>
                </li>
            </ul>
        </div>
        <div class="block">
            <router-view></router-view>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import type { IUser } from '@/api/types';

export default defineComponent({
    name: 'Home',
    data(){
        return {
            userId: 0,
            activeTab: 0
        }
    },
    created() {
        this.userId = (JSON.parse(localStorage.getItem('user') ?? '{}') as IUser).id;

    },
    methods: {
        selectTab(id: number){
            this.activeTab = id;
        }
    }
});

</script>

<style>

.menu{
   user-select: none;
    -moz-user-select: none;
    -webkit-user-select: none;
    -ms-user-select: none;
    cursor: default;
}
.menu li{
   list-style-type: none;
   margin-top: 5px;
   display: block;
}

.menu ul {
    margin-top: 10px;
}

.main {
    display: flex;
    width: 80%;
}

.block {
    margin-top: 10px;
    border-bottom-left-radius: 15px;
    border-bottom-right-radius: 15px;
    border-top-right-radius: 15px;
    background-color: #222222;
    width: 100%;
    height: 80%;
}

.active {
    background-color: #222222;
    border-bottom-left-radius: 10px;
    border-top-left-radius: 10px;
}


</style>