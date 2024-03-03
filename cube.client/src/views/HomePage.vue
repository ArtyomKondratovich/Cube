<template>
   <div class="main">
    <div class="menu">
        <li>
            <div class="profile">
                
            </div>
        </li>
        <li>
            <button @click="nextBlock('posts')">Home</button>
        </li>
        <li>
            <button @click="nextBlock('messages')">Messages</button>
        </li>
        <li>
            <button @click="nextBlock('notifications')">Notifications</button>
        </li>
        <li>
            <button @click="nextBlock('friends')">Friends</button>
        </li>
        <li>
            <button @click="nextBlock('settings')">Settings</button>
        </li>
    </div>
    <div class="block">
        <div v-if="block === 'messages'">
            <Chats />
        </div>

        <div v-if="block === 'settings'">
          <Settings />
        </div>

        <div v-if="block === 'posts'">
          <Posts />
        </div>

        <div v-if="block === 'friends'">
          <Friends />
        </div>
    </div>
   </div>
</template>

<script lang="ts">
import { defineComponent, type Component } from 'vue';
import Posts from '@/components/Posts.vue';
import Chats from "@/components/Chats.vue";
import Settings from "@/components/Settings.vue";
import Friends from "@/components/Friends.vue";

export default defineComponent({
    name: 'Home',
    data(){
        return {
            block: 'posts'
        }
    },
    components: {
        Chats,
        Posts,
        Settings,
        Friends
    },
    methods: {
        nextBlock(next: string): void {
            this.block = next;
        }
    }
});

</script>

<style>
.main {
    display: flex;
}
  
.block {
    display: inline-block;
}

.menu{
   width: 150px;
   height: 200px;
   border: solid 1px grey;
   border-radius: 10px;
}
.menu li{
   align-items: center;
   list-style-type: none;
   margin-top: 5px;
}
</style>