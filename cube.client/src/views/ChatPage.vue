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
                        <li v-for="message in chat.messages" :key="message.id">
                            <div :class="getMessageClass(message.sender.id)">
                                <h3>{{ message.message }}</h3>
                                <h3>{{ message.createdDate }}</h3>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="sender">
                    <form @submit="sendMessage">
                        <input type="text" v-model="message" placeholder="Type your message..." />
                        <button type="submit">Send</button>
                    </form>
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
            message: '',
            chat: {} as IChat,
            loading: true
        }
    },
    mounted() {
        this.chat.id = Number(this.$route.params.chatId);
        this.userId = (JSON.parse(localStorage.getItem('user') ?? '{}') as IUser).id;

        let id = this.chat.id;

        axios.post(`${config.apiUrl}/Chat/Get`, { id }, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        })
        .then(async (response) => {
            const data = response.data as IResponse<IChat>;

            if (data.responseResult == 'Success' && data.value) {
                this.chat = data.value;
                this.loading = false;
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
    methods: {
        getMessageClass(senderId: number): string {
            let isUsersMessage = this.userId == senderId;
            return isUsersMessage ? 'message-right' : 'message-left';
        },
            sendMessage(){
            
            let messageInput = {
                senderId: this.userId,
                chatId: this.chat.id,
                message: this.message
            } as IMesssageInput;

            axios.post(`${config.apiUrl}/Message/send`, { messageInput }, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                }
            })
            .then((response) => {
                const data = response.data as IResponse<IMessage>;

                if (data.responseResult == 'Success' && data.value){
                    this.chat.messages.push(data.value);
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
    .main div {
        display: inline-block;
    }

    .main {
        width: 80%;
    }

    .chat {
        border: 0.5px solid grey;
        border-radius: 15px;
        margin-left: 10px;
        width: 80%;
    }

    .message-right{
        text-align: right;
    }

    .message-left
    {
        text-align: left;
    }

</style>