<template>
    <div class="main">
        <div class="menu">
            <ul>
                <li>
                <div class="profile">
                    Profile
                </div>
                </li>
                <li>
                    <router-link :to="{ path: '/home/posts' }">Home</router-link>
                </li>
                <li>
                    <router-link :to="{ path: '/home/messages' }">Messages</router-link>
                </li>
                <li>
                    <div>Notifications</div>
                </li>
                <li>
                    <div>Friends</div>
                </li>
                <li>
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
        }
    },
    created() {
        this.userId = (JSON.parse(localStorage.getItem('user') ?? '{}') as IUser).id;

    },
    methods: {
    }
});

</script>

<style>

.menu{
   width: 150px;
   height: 200px;
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
   padding-left: 10px;
}

.menu ul {
    padding-left: 0px;
}

.main {
    display: flex;
    width: 80%;
}

.block {
    margin-top: 10px;
    border-radius: 15px;
    background-color: #222222;
    width: 100%;
    height: 80%;
}


</style>