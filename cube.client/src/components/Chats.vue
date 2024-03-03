<template>
    <div class="chats" style="overflow-y: scroll; height: 400px">
        <ul>
            <li v-for="chat in chats">
                <div @click="">
                    <h3>{{chat.title}}</h3>
                    <p>{{chat.chatType}}</p>
                </div>
            </li>
        </ul>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useChatStore } from '@/store/chat.store';
import type { IChat } from '@/api/types';

export default defineComponent({
    name: "Chats",
    data() {
        return {
            chats: [] as IChat[]
        }
    },
    async created() {
        await this.loadChats();
    },
    methods: {
        async loadChats(){
            const store = useChatStore();
            await store.loadChats();
            this.chats = store.getChats;
        }
    }
});

</script>

<style>
</style>