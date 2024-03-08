<template>
    <div class="menu">
        <li>
            <div class="profile">
                Profile
            </div>
        </li>
        <li>
            <div @click="navigateTo('/home')">Home</div>
        </li>
        <li>
            <div @click="navigateTo('/messages')">Messages</div>
        </li>
        <li>
            <div @click="navigateTo('/notifications')">Notifications</div>
        </li>
        <li>
            <div @click="navigateTo('/friends')">Friends</div>
        </li>
        <li>
            <div @click="navigateTo('/settings')">Settings</div>
        </li>
    </div>
</template>

<script lang="ts">
import router from '@/helpers/router';
import { defineComponent } from 'vue';

export default defineComponent({
    name: 'Menu',
    methods: {
        navigateTo(path: string) {
            router.push(path);
        }
    }
});
</script>

<style>
.menu{
   width: 150px;
   height: 200px;
   border: solid 1px grey;
   border-radius: 15px;
}
.menu li{
   list-style-type: none;
   margin-top: 5px;
   display: block;
   padding-left: 10px;
}

.menu {
    user-select: none;
    -moz-user-select: none;
    -webkit-user-select: none;
    -ms-user-select: none;
    cursor: default;
}
</style>