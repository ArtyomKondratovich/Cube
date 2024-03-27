<template>
    <div class="messanger">
        <div style="display: flex; flex-basis: 30%;">
            <ChatsBlock @select-chat="selectChat"/>
        </div>
        <div v-if="!isAnyChatSelected" class="selectAnyChat">
            <p>Select any chat</p>
        </div>
        <div v-if="isAnyChatSelected" style="display: flex; flex-basis: 70%;">
            <router-view></router-view>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { ref } from 'vue'
    import ChatsBlock from './ChatsBlock.vue';
    import router from '@/helpers/router';


    const isAnyChatSelected = ref(false);
    const selectedChat = ref<number>(0);
    
    function selectChat(chatId: number) {
        selectedChat.value = chatId;
        isAnyChatSelected.value = true;
        router.push({ path: `/messanger/chat/${chatId}` })
    }

</script>


<style>
    .messanger {
        display: flex;
        width: 100%;
    }

    .selectAnyChat{
        display: flex;
        align-items: center;
        justify-content: center;
        flex-basis: 70%;
    }

</style>