<template>
    <div class="main">
        <div>
            <Menu></Menu>
        </div>
        <div class="chat">
            <div v-if="loading">
                <p>Loading messages...</p>
            </div>
            <div v-if="!loading">
                <div class="header">

                </div>
                <div class="messages">
                    <ul>
                        <li v-for="message in messages" :key="message.id">
                            <div :class="getMessageClass(message.userId)">
                                <h3>{{ message.message }}</h3>
                                <h3>{{ message.createdDate }}</h3>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="sender">
                    <form @submit.prevent="sendMessage">
                        <div class="editableDiv" contenteditable="true" data-placeholder="Type your message..." inputmode="text" translate="no"></div>
                    </form>
                    <button type="submit">
                            <img src="../assets/icons/sendIcon.png"/>
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import type { 
    IChat,
    IMessage,
    IMesssageInput,
    IResponse, 
    IUser
} from '@/api/types';
import Menu from '@/components/Menu.vue';
import { toast } from 'vue3-toastify';
import axios from 'axios';
import config from '@/config';

export default defineComponent({
    name: "Chat",
    components: {
        Menu
    },
    data() {
        return {
            userId: 0,
            chat: {} as IChat,
            message: '',
            messages: [] as IMessage[],
            loading: true
        }
    },
    created() {
        this.chat.id = Number(this.$route.params.chatId);
        this.userId = (JSON.parse(localStorage.getItem('user') ?? '{}') as IUser).id;
        this.loadChatData(this.chat.id);
        this.loadMessages();
    },
    methods: {
        getMessageClass(senderId: number): string {
            let isUsersMessage = this.userId == senderId;
            return isUsersMessage ? 'message-right' : 'message-left';
        },
        loadChatData(id: number) {
            axios.post(`${config.apiUrl}/Chat/getChat`, { id }, {
                headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
                }
            })
            .then(async (response) => {
                const data = response.data as IResponse<IChat>;

                if (data.responseResult == 'Success' && data.value) {
                    this.chat.users = data.value.users;
                }
                else{
                    toast.error(data.responseResult);
                    await new Promise(resolve => setTimeout(resolve, 2000));
                }})
            .catch(async error => {
                toast.error(error);
                await new Promise(resolve => setTimeout(resolve, 2000));
            });
        },
        loadMessages() {
            let id = this.chat.id;
            axios.post(`${config.apiUrl}/Message/getChatMessages`, { id } , {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                }
            })
            .then((response) => {
                const data = response.data as IResponse<IMessage[]>;

                if (data.responseResult == 'Success' && data.value){
                    this.messages = data.value;
                    this.loading = false;
                }
                else
                {
                    toast.error(data.responseResult);
                }
            })
            .catch(error => {
                toast.error('server error');
            })
        },
        sendMessage(){
            const message = document.
            const messageInput = {
                senderId: this.userId,
                chatId: this.chat.id,
                message: this.message
            } as IMesssageInput;

            axios.post(`${config.apiUrl}/Message/send`, messageInput, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                }
            })
            .then((response) => {
                const data = response.data as IResponse<IMessage>;

                if (data.responseResult == 'Success' && data.value){
                    this.messages.push(data.value);
                }
                else
                {
                    toast.error(data.responseResult);
                }
            })
            .catch(error => {
                toast.error('server error');
            })
        }
    }
});

</script>

<style>
    .main {
        display: flex;
        width: 80%;
    }

    .chat {
        display: flex;
        
    }

    .sender{
        display: inline;
        background-color: #292929;
        border-radius: 10px;
    }

    .sender form {
        display: flex;
        width: 100%;
    }

    .editableDiv {
        margin-left: 10px;
        flex: 1;
        flex-direction: column-reverse;
        overflow-y: auto;
        resize: none;
        overflow: hidden;
        border: none;
        box-sizing: border-box;
        height: auto;
        padding: 5px;
    }

    #editableDiv {
        position: relative;
    }

    .editableDiv::before {
        content: attr(data-placeholder);
        position: absolute;
        color: #aaa;
        pointer-events: none;
    }

    .editableDiv.empty::before {
      display: block;
    }

    .editableDiv:focus::before {
      display: none;
    }

    form button {
        display: flex;
        align-items: center;
        padding: 0px;
        background: none;
        border: none;
        flex: 0;
    }

    .message-right{
        text-align: right;
    }

    .message-left
    {
        text-align: left;
    }

    img:hover {
        color: #aaa;
    }


</style>