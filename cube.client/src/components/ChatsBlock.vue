<template>
    <div class="chats">
            <div>
                Search bar
            </div>
            <div v-if="loading" class="spinner">
                <VueSpinner size="20"/>
            </div>
            <div v-if="!loading">
                <ul>
                    <li v-for="chat in chats" :key="chat.id">
                        <div class="chat" @click="emit('select-chat', chat.id)">
                            <p>{{chat.title}}</p>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
</template>

<script setup lang="ts">
import { VueSpinner } from 'vue3-spinners';
import { ref } from 'vue';
import type {
    IChatLoad,
    IResponse
} from '@/api/types';
import { toast } from 'vue3-toastify';
import axios from 'axios';
import config from '@/config';
import { defineEmits } from 'vue';

const props = defineProps({
    userId: {
        type: Number,
        required: true
    }
});

const emit = defineEmits<{
    (e: 'select-chat', id: number): void
}>();

const loading = ref(true);
const chats = ref<IChatLoad[]>([]);

axios.post(`${config.apiUrl}/Chat/getUserChats`, { Id: props.userId }, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        })
        .then(async (response) => {
            const data = response.data as IResponse<IChatLoad[]>;
            if (data.responseResult == 'Success' && data.value){
                chats.value = data.value;
                loading.value = false;
            }
            else{
                toast.error(data.responseResult);
                await new Promise(resolve => setTimeout(resolve, 2000));
            }})
            .catch(async error => {
                toast.error(error);
                await new Promise(resolve => setTimeout(resolve, 2000));
        });
</script>

<style>
    .chats {
        flex-basis: 30%;
        width: 100%;
        height: 100%;
        border-right: solid 1px #363738;
        font-size: small;
        font-family: 'Robotic';
    }

    .spinner {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
        height: 100%;
    }

    .chats ul {
        list-style: none;
        margin-top: 5px;
        margin-bottom: 5px;
        padding-left: 5px;
        padding-right: 5px;
    }

    li {
        display: flex;
        align-items: center;
    }

    .chat {
        display: flex;
        width: 100%;
        border-radius: 15px;
        padding-left: 10px;
        padding-right: 10px;
        user-select: none;
        -moz-user-select: none;
        -webkit-user-select: none;
        -ms-user-select: none;
    }

    .chat:hover {
        background-color: #2B2B2B;
    }

    .image {
        vertical-align: middle;
        width: 50px;
        height: 50px;
    }
</style>