<template>
    <div class="chats" v-if="!chatCreation">
        <div style="display: flex; flex-direction: row;">
            <div class="searchHeader">
                <div id="searchBar1">
                    <input type="text" placeholder="Search" v-model="searchText">
                    <img src="../assets/icons/searchIcon.png">
                </div>
            </div>
            <div id="create-chat" @click="startChatCreation">
                <img src="../assets/icons/editIcon.png" style="width: 24px; height: 24px;">
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
                        <img v-if="chat.type == 'Private' && companionAvatar(chat)" :src="chatAvatar(chat)">
                        <img v-if="chat.type == 'Group'" src="../assets/icons/groupChatIcon.png">
                        <p style="margin-left: 5px;">{{ chatTitle(chat) }}</p>
                        <div class="circle" v-if="store.getChatNotifications(chat.id).length != 0">
                            <span class="number">{{ store.getChatNotifications(chat.id).length }}</span>
                        </div>
                    </router-link>
                </li>
            </ul>
        </div>
    </div>
    <div v-if="chatCreation" class="ChatCreation">
        <div class="ChatCreation_header">
            <div id="header-text">
                Create chat
            </div>
            <div id="header-image">
                <img src="../assets/icons/exitIcon.png" @click="() => chatCreation = false">
            </div>
        </div>
        <div class="ChatCreation_form">
            <div id="chat-image">
                <img src="../assets/icons/groupChatIcon.png" style="width: 48px;height: 48px; border-radius: 24px; border: 1px solid #363738;">
            </div>
            <div id="chat-name">
                <input type="text" placeholder="Enter the chat name" v-model="newChat.title">
                <span style="font-size: 13px; word-wrap: normal;">
                    Enter a name and upload an optional photo
                </span>
            </div>
        </div>
        <div class="ChatCreation_selector" v-if="!loading">
            <ul>
                <li v-for="friend in userFriends" :key="friend.id">
                    <div id="user-profile">
                        <img v-if="friend.avatarBytes == null" src="../assets/Images/Profile/Profile_default.png">
                        <img v-if="friend.avatarBytes != null" :src="userAvatar(friend.avatarBytes)">
                        <div class="userName"> {{ friend.name }} {{ friend.surname }} </div>
                    </div>
                    <div id="selec-user">
                        <input
                        type="checkbox"
                        :value="friend"
                        v-model="selectedFriends"
                    />
                    </div>
                </li>
            </ul>
        </div>
        <div class="ChatCreation_footer">
            <button @click="createChat" :disabled="!createButtonActive">Create</button>
        </div>
    </div>
</template>

<script setup lang="ts">
import { VueSpinner } from 'vue3-spinners';
import { ref, computed, inject } from 'vue';
import {
    type IResponse,
    type IChat,
    type IUser,
type IChatInput,
ChatType
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
const chatCreation = ref(false);
const userFriends = ref([] as IUser[]);
const newChat = ref<IChatInput>({} as IChatInput);
const selectedFriends = ref([] as IUser[]);
const createButtonActive = computed(() => {
    return selectedFriends.value.length > 1 && newChat.value.title != '';
});

async function fetchUserFriends(): Promise<void>{
    if (chatCreation.value){
        axios.post(`${config.apiUrl}/User/getUserFriends`, { id: userId.value }, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'Application/json'
            }
        }).then(response => {
            const data = response.data as IResponse<IUser[]>;

            if (data.responseResult == 'Success' && data.value)
            {
                userFriends.value = data.value;
            }
        }).catch(error => console.log(error));
    }
}

const filteredChats = computed(() => {
  if (!searchText.value) {
    return chats.value;
  }
  const searchQuery = searchText.value.toLowerCase();
  return chats.value.filter(chat => chat.title.toLowerCase().includes(searchQuery));
});

onMounted(async () => {
    loading.value = true;
    await fetchUserChats();
    loading.value = false;
});

async function startChatCreation(): Promise<void> {
    loading.value = true;
    chatCreation.value = true;
    await fetchUserFriends();
    loading.value = false;
}

async function createChat(): Promise<void> {
    loading.value = true;
    newChat.value.type = ChatType.Group;
    let ids = selectedFriends.value.map(x => x.id);
    ids.push(userId.value);
    newChat.value.patricipantsIds = ids;

    await axios.post(`${config.apiUrl}/Chat/create`, newChat.value , {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        }
    }).then(response => {
        const data = response.data as IResponse<IChat>;
        if (data.responseResult == 'Success' && data.value){
            chats.value.push(data.value);
            toast.success(`Create new chat ${data.value.title}`);
            chatCreation.value = false;
        }
    }).catch(error => console.log(error));

    loading.value = false;
}

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

function chatAvatar(chat: IChat): string{
    let companion = chat.users.find(x => x.id != userId.value) as IUser;
    return 'data:image/jpeg;base64,' + companion.avatarBytes;
}

function userAvatar(bytes: []): string {
    return 'data:image/jpeg;base64,' + bytes;
}

async function fetchUserChats() {
    axios.post(`${config.apiUrl}/Chat/getUserChats`, { Id: userId.value }, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        })
        .then(async (response) => {
            const data = response.data as IResponse<IChat[]>;

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

    #create-chat {
        display: flex;
        margin: 5px;
        cursor: pointer;
    }

    #create-chat:hover {
        background-color: #363738;
        border-radius: 5px;
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

    .ChatCreation {
        display: flex;
        flex-direction: column;
        width: 100%;
        height: 100%;
        border-right: 1px solid #363738;
    }

    .ChatCreation_header {
        display: flex;
        width: 100%;
        flex-direction: row;
    }

    #header-text {
        display: flex;
        align-items: center;
        margin-left: 10px;
        flex-grow: 1    ;
    }

    #header-image {
        display: flex;
        align-items: center;
        justify-items: center;
        cursor: pointer;
        margin: 5px;
    }

    #user-profile {
        display: flex;
        width: 100%;
        flex-direction: row;
    }

    #user-profile img{
        width: 36px;
        height: 36px;
        border-radius: 18px;
    }

    #chat-image {
        padding: 5px;
    }

    #chat-name {
        padding: 5px;
    }

    #chat-name input {
        background-color: #363738;
        padding: 5px;
        border-radius: 5px;
        color: white;
        border: none;
        appearance: none;
        -webkit-appearance: none;
        -moz-appearance: none;
        -ms-appearance: none;
        outline: none;
    }

    #header-image:hover {
        background-color: #363738;
        border-radius: 5px;
    }

    .ChatCreation_form {
        display: flex;
        border-bottom: 1px solid #363738;
        padding: 5px;
    }

    .ChatCreation_selector {
        display: flex;
        height: 100%;
        overflow-y: scroll;
        border-bottom: 1px solid #363738;
        flex-wrap: 1;
    }

    .ChatCreation_selector ul {
        padding: 0px;
        margin: 0px;
        width: 100%;
    }

    .ChatCreation_selector li {
        padding: 5px;
        width: auto;
    }

    .ChatCreation_selector::-webkit-scrollbar { 
        width: 10px;
    } 

    .ChatCreation_selector::-webkit-scrollbar-track { 
        background: #222222;
        border-radius: 5px;

    } 

    .ChatCreation_selector::-webkit-scrollbar-thumb { 
        background: #292929;
        border-radius: 5px;
    } 

    .ChatCreation_selector::-webkit-scrollbar-thumb:hover { 
        background: #555; 
    } 

    .ChatCreation_selector::-webkit-scrollbar-button { 
        display: none; 
    }

    .ChatCreation_footer {
        display: flex;
        padding: 5px;
        justify-content: flex-end;
        
    }

    .ChatCreation_footer button {
        border: none;
        appearance: none;
        -webkit-appearance: none;
        -moz-appearance: none;
        -ms-appearance: none;
        outline: none;
        padding: 5px;
        border-radius: 5px;
        background-color: #363738;
        color: white;
    }

    .ChatCreation_footer button:disabled {
        cursor:not-allowed;
    }
</style>