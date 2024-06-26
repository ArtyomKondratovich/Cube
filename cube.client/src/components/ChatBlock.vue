<template>
    <div class="chatView">
            <div class="header">
                <div style="margin-left: 5px;">
                    {{ chatTitle(chat) }}
                </div>
                <div v-if="chat.type == 'Group'" style="margin-left: 5px; font-size: 11px;">
                    {{ chat.users.length }} members
                </div>
            </div>
            <div v-if="loading" class="loadingSpinner">
                <VueSpinner :size="20"/>
            </div>
            <div id="messageContainer" v-if="!loading" class="messages">
                <ul>
                    <li v-for="message in chatMessages" :key="message.id" :class="getMessageClass(message.userId)">
                        <div id="text">
                            {{ message.message }}
                        </div>
                        <div id="date">
                            {{ message.formatedCreatedDate }}
                        </div>
                    </li>
                </ul>
            </div>
            <div class="sender">
                <form @submit.prevent="sendMessage">
                    <textarea v-model="message" placeholder="Type your message..." @keydown.enter.prevent="sendMessage" rows="1"></textarea>
                    <button type="submit">
                        <img src="../assets/icons/sendIcon.png" />
                    </button>
                </form>
            </div>
        </div>
</template>

<script setup lang="ts">
import { VueSpinner } from 'vue3-spinners';
import { ref, onMounted, nextTick, inject, watch, toRef } from 'vue';
import type { 
    IChat,
    IMessage,
    IMesssageInput,
    IResponse
} from '@/api/types';
import { toast } from 'vue3-toastify';
import axios from 'axios';
import config from '@/config';
import getUserFromLocalStorage from '@/helpers/getFromLocalStorage';
import type { INotificationStore } from '@/store/notification.store';

const props = defineProps({
    chatId: {
        type: Number,
        required: true
    }
});

const userId = ref(getUserFromLocalStorage())
const chat = ref({} as IChat);
const message = ref('');
const chatMessages = ref<IMessage[]>([]);
const loading = ref(true);
const timeZoneOffset = ref((new Date().getTimezoneOffset()) / 60 * (-1));
const notificationStore = inject<INotificationStore>('notificationStore') as INotificationStore;

onMounted(() => {
    loadChatData(props.chatId);
    readChatNotifications();
});

function chatTitle(chat: IChat): string{
    if (chat.type == 'Private'){
        let companion = chat.users.filter(x => x.id != userId.value);

        return `${ companion[0].name } ${ companion[0].surname }`;
    }

    return chat.title;
}



function loadChatData(id: number){
    fetchChat(id);
    fetchChatMessages(id);
}

async function readChatNotifications() {
    const notifications = notificationStore.getChatNotifications(props.chatId);
    const ids = notifications.map(x => x.id);
    notificationStore.readNotifications(ids);
}

function fetchChat(id: number) {
    axios.post(`${config.apiUrl}/Chat/getChat`, 
        { Id: id }, 
        {
            headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
            }
        })
        .then(async (response) => {
            const data = response.data as IResponse<IChat>;
            if (data.responseResult == 'Success' && data.value) {
                chat.value = data.value;
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
 
function fetchChatMessages(id: number) {
    axios.post(`${config.apiUrl}/Message/getChatMessages`, 
    { 
        ChatId: id,
        take: 10,
        skip: 0
    }, 
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        }
    })
    .then((response) => {
        const data = response.data as IResponse<IMessage[]>;
        if (data.responseResult == 'Success' && data.value){
            chatMessages.value = data.value;
            loading.value = false;
            nextTick(() => {    
                scrollToBottom();
            });
        }
        else
        {
            toast.error(data.responseResult);
        }
    })
    .catch(error => {
        toast.error('server error');
    });
}

function getMessageClass(senderId: number): string {
    let isUsersMessage = userId.value == senderId;
    return isUsersMessage ? 'message-right' : 'message-left';
}

function scrollToBottom(): void {
    let container = document.querySelector('#messageContainer');

    if (container != null && chatMessages.value.length > 0) {
        container.scrollTop = container.scrollHeight;
    }
}

function sendMessage(): void {
    const messageInput = {
        senderId: userId.value,
        chatId: props.chatId,
        message: message.value,
        timeZoneOffset: timeZoneOffset.value
    } as IMesssageInput;

    message.value = '';

    axios.post(`${config.apiUrl}/Message/send`, 
        messageInput, 
        {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        })
        .then((response) => {
            const data = response.data as IResponse<IMessage>
            if (data.responseResult == 'Success' && data.value) {
                chatMessages.value.push(data.value);
                nextTick(() => {
                    scrollToBottom();
                });
            }
            else {
                toast.error(data.responseResult);
            }
        })
        .catch(error => {
            toast.error('server error');
        });
    
    axios.post(`${config.apiUrl}/Notification/create`, {
            notificationSenderId: chat.value.id,
            userIds: chat.value.users.filter(x => x.id != userId.value).map(x => x.id),
            isReaded: false,
            type: 'ChatNotification',
            accepted: false
        },
        {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        }).then(response => {

        });
}

watch(toRef(props, 'chatId'), async (newChatId) => {
    loadChatData(newChatId);
});

</script>

<style scoped>
    .chatView {
        display: flex;
        width: 100%;
        flex-direction: column;
        font-size: small;
    }

    .messages {
        display: flex;
        flex-basis: 90%;
        overflow-y: scroll;
    }

    .loadingSpinner {
        display: flex;
        justify-content: center;
        align-items: center;
        flex-basis: 90%;
    }

    .chatView .header {
        flex-basis: 10%;
        max-height: 36px;
        padding: 5px;
        flex-direction: column;
        border-bottom: 1px solid #363738;
    }

    .chatView .messages li {
        max-width: 50%;
        word-wrap: break-word;
        align-items: normal;
        padding: 5px;
        border-radius: 15px;
        margin: 5px;
        background-color: #2A2F33;
        flex-direction: column;
    }

    #text {
        text-align: left;
    }

    #date {
        text-align: right;
        font-size: 10px;
    }

    .messages ul {
        margin: 10px;
        display: flex;
        padding: 0px;
        width: 100%;
        flex-direction: column;
    }

    .messages::-webkit-scrollbar { 
        width: 10px;
    } 

    .messages::-webkit-scrollbar-track { 
        background: #222222;
        border-radius: 5px;

    } 

    .messages::-webkit-scrollbar-thumb { 
        background: #292929;
        border-radius: 5px;
    } 

    .messages::-webkit-scrollbar-thumb:hover { 
        background: #555; 
    } 

    .messages::-webkit-scrollbar-button { 
        display: none; 
    }

    .message-right{
        align-self: flex-end;
    }

    .message-left
    {
        align-self: flex-start;
    }

    .sender {
        display: flex;
        flex-direction: row;
        margin: 5px;
        padding-left: 15px;
        background-color: #292929;
        border-radius: 10px;
    }

    .sender form {
        width: 100%;
        display: flex;
        flex-direction: row;
    }

    .sender form textarea {
        flex-grow: 1;
        max-height: 150px;
        outline: none;
        resize: none;
        border: none;
        overflow-y: hidden;
        color: white;
        background-color: #292929;
        font-family: 'Robotic';
        padding: 5px;
    }

    .sender form button {
        align-self: flex-end;
        display: flex;
        align-items: center;
        padding: 0px;
        background: none;
        border: none;
        width: 30px;
        height: 30px;
    }

</style>