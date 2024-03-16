<template>
    <div class="chatView">
            <div class="header">

            </div>
            <div v-if="loading" class="loadingSpinner">
                <VueSpinner :size="20"/>
            </div>
            <div v-if="!loading" class="messages">
                <ul>
                    <li v-for="message in chatMessages" :key="message.id" :class="getMessageClass(message.userId)">
                        <div id="text">
                            {{ message.message }}
                        </div>
                        <div id="date">
                            {{ message.createdDate }}
                        </div>
                    </li>
                </ul>
            </div>
            <div class="sender">
                <form @submit.prevent="sendMessage">
                    <textarea v-model="message" placeholder="Type your message..." rows="1"></textarea>
                    <button type="submit">
                        <img src="../assets/icons/sendIcon.png" />
                    </button>
                </form>
            </div>
        </div>
</template>

<script setup lang="ts">
import { VueSpinner } from 'vue3-spinners';
import { ref } from 'vue';
import type { 
    IChat,
    IMessage,
    IMesssageInput,
    IResponse
} from '@/api/types';
import { toast } from 'vue3-toastify';
import axios from 'axios';
import config from '@/config';

const props = defineProps({
    chatId: {
        type: Number,
        required: true
    },
    userId: {
        type: Number,
        required: true
    }
});

const chat = ref({} as IChat);
const message = ref('');
const chatMessages = ref<IMessage[]>([]);
const loading = ref(true);

axios.post(`${config.apiUrl}/Chat/getChat`, 
        { Id: props.userId }, 
        {
            headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
            }
        })
        .then(async (response) => {
            const data = response.data as IResponse<IChat>  
            if (data.responseResult == 'Success' && data.value) {
                chat.value.users = data.value.users;
            }   
            else{
                toast.error(data.responseResult);
                await new Promise(resolve => setTimeout(resolve, 2000));
            }})
        .catch(async error => {
            toast.error(error);
            await new Promise(resolve => setTimeout(resolve, 2000));
        });
 
axios.post(`${config.apiUrl}/Message/getChatMessages`, 
    { Id: props.chatId }, 
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
        }
        else
        {
            toast.error(data.responseResult);
        }
    })
    .catch(error => {
        toast.error('server error');
    });

function getMessageClass(senderId: number): string {
    let isUsersMessage = props.userId == senderId;
    return isUsersMessage ? 'message-right' : 'message-left';
}

function sendMessage(): void {
    const messageInput = {
        senderId: props.userId,
        chatId: props.chatId,
        message: message.value
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
            }
            else {
                toast.error(data.responseResult);
            }
        })
        .catch(error => {
            toast.error('server error');
        });
}
</script>

<style>
    .chatView {
        flex-basis: 70%;
        display: flex;
        flex-direction: column;
        font-size: small;
    }

    .messages {
        flex-basis: 80%;
        overflow-y: scroll;
        display: flex;
    }

    .loadingSpinner {
        display: flex;
        justify-content: center;
        align-items: center;
        flex-basis: 80%;
    }

    .chatView .header {
        flex-basis: 15%;
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
        align-items: flex-end;
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
        height: auto;
        max-height: 100px;
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
        display: flex;
        align-items: center;
        padding: 0px;
        background: none;
        border: none;
    }

</style>