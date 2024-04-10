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
                        <router-link class="chat" :to="{ path: `/messanger/chat/${chat.id}`}">
                            <img v-if="chat.type == 'SavedMessages'" src="../assets/icons/pinIcon.png">
                            <img v-if="chat.type == 'Private' && !companionAvatar(chat)" src="../assets/Images/Profile/Profile_default.png">
                            <img v-if="chat.type == 'Private' && companionAvatar(chat)" :src="userAvatar(chat)">
                            <p style="margin-left: 5px;">{{ chatTitle(chat) }}</p>
                            <div class="circle" v-if="store.getChatNotifications(chat.id).length != 0">
                                <span class="number">{{ store.getChatNotifications(chat.id).length }}</span>
                            </div>
                        </router-link>
                    </li>
                </ul>
            </div>
        </div>
</template>

<script setup lang="ts">
import { VueSpinner } from 'vue3-spinners';
import { ref, computed, inject } from 'vue';
import type {
    IChatLoad,
    IResponse,
    IChat,
    IUser
} from '@/api/types';
import { toast } from 'vue3-toastify';
import axios from 'axios';
import config from '@/config';
import { onMounted } from 'vue';
import getUserIdFromLocalStorage from '@/helpers/getFromLocalStorage';
import { type INotificationStore } from '@/store/notification.store';

const userId = ref(getUserIdFromLocalStorage());
const loading = ref(true);
const chats = ref<IChat[]>([]);
const searchText = ref('');
const store = inject<INotificationStore>('notificationStore') as INotificationStore;

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

function chatTitle(chat: IChat): string{
    if (chat.type == 'Private'){
        let companion = chat.users.filter(x => x.id != userId.value);

        return `${ companion[0].name } ${ companion[0].surname }`;
    }

    return chat.title;
}

function companionAvatar(chat: IChat): boolean{
    let companion = chat.users.find(x => x.id != userId.value) as IUser;
    return companion.avatarBytes != null;
}

function userAvatar(chat: IChat): string{
    let companion = chat.users.find(x => x.id != userId.value) as IUser;
    return 'data:image/jpeg;base64,' + companion.avatarBytes;
}

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
    .circle {
      width: 18px;
      height: 18px;
      background-color: red;
      border-radius: 9px;
      margin-left: 10px;
      display: flex;
      justify-content: center;
      align-items: center;
    }

    .number {
      font-size: small;
      font-weight: bold;
      color: white;
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
        align-items: center;
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