<template>
    <div class="chats" style="overflow-y: scroll; height: 400px">
        <ul>
            <li v-for="chat in chats" :key="chat.id">
                <div @click="stepIntoChat(chat.id)">
                    <h3>{{chat.title}}</h3>
                    <p>{{chat.type}}</p>
                </div>
            </li>
        </ul>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useChatStore } from '@/store/chats.store';
import type { IChatLoad } from '@/api/types';

export default defineComponent({
    name: "ChatsComponent",
    data() {
        return {
            chats: [] as IChatLoad[]
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
        },
        stepIntoChat(id: number): void{
            console.log(id);
        }
    }
});

</script>

<style>
    
</style>