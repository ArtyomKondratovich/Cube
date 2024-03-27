<template>
    <div class="chats">
            <div>
                <div class="searchHeader">
                    <div id="searchBar1">
                        <input type="text" placeholder="Search" v-model="searchText">
                        <img src="../assets/icons/searchIcon.png">
                    </div>
                </div>
            </div>
            <div v-if="loading" class="spinner">
                <VueSpinner size="20"/>
            </div>
            <div v-if="!loading" class="chatsList">
                <ul>
                    <li v-for="chat in filteredChats" :key="chat.id">
                        <div class="chat" @click="emit('select-chat', chat.id)">
                            <img src="../assets/icons/pinIcon.png">
                            <p>{{chat.title}}</p>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
</template>

<script setup lang="ts">
import { VueSpinner } from 'vue3-spinners';
import { ref, computed } from 'vue';
import type {
    IChatLoad,
    IResponse
} from '@/api/types';
import { toast } from 'vue3-toastify';
import axios from 'axios';
import config from '@/config';
import { defineEmits, onMounted } from 'vue';
import getUserIdFromLocalStorage from '@/helpers/getFromLocalStorage';

const emit = defineEmits<{
    (e: 'select-chat', id: number): void
}>();

const userId = ref(getUserIdFromLocalStorage());
const loading = ref(true);
const chats = ref<IChatLoad[]>([]);
const searchText = ref('');

const filteredChats = computed(() => {
  if (!searchText.value) {
    return chats.value;
  }
  const searchQuery = searchText.value.toLowerCase();
  return chats.value.filter(chat => chat.title.toLowerCase().includes(searchQuery));
});

onMounted(async () => {
    await fetchUserChats();
});

async function fetchUserChats() {
    axios.post(`${config.apiUrl}/Chat/getUserChats`, { Id: userId.value }, {
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
} 

</script>

<style>
    .active {
        border-radius: 0px;
        background-color: #141414;
    }
    .chats {
        width: 100%;
        height: 100%;
        border-right: solid 1px #363738;
        font-size: small;
        font-family: 'Robotic';
    }

    .chatsList img {
        width: 36px;
        height: 36px;
        border-radius: 18px;
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

    .chatsList li:hover {
        background-color: #363738;
        border-radius: 10px;
    }

    .image {
        vertical-align: middle;
        width: 50px;
        height: 50px;
    }
</style>