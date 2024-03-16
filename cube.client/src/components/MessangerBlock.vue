<template>
    <div class="messanger">
        <ChatsBlock :userId="userId" @select-chat="selectChat"/>
        <div v-if="!isAnyChatSelected" class="selectAnyChat">
            <p>Select any chat</p>
        </div>
        <ChatBlock v-if="isAnyChatSelected" :chatId="selectedChat" :userId="userId"/>
    </div>
</template>

<script setup lang="ts">
    import { ref, defineProps } from 'vue'
    import ChatBlock from './ChatBlock.vue';
    import ChatsBlock from './ChatsBlock.vue';

    const props = defineProps({
        userId: {
            type: Number,
            required: true
        }
    });

    const isAnyChatSelected = ref(false);
    const selectedChat = ref<number>(0);
    
    function selectChat(id: number) {
        selectedChat.value = id;
        isAnyChatSelected.value = true;
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