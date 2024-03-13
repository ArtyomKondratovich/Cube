<template>
    <div class="chatView">
            <div class="header">

            </div>
            <div v-if="loading">
                <p>Loading messages...</p>
            </div>
            <div v-if="!loading" class="messages">
                <ul>
                    <li v-for="message in messages" :key="message.id" :class="getMessageClass(message.userId)">
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
                    <textarea v-model="message" ref="messageInput" placeholder="Type your message..." rows="1"></textarea>
                    <button type="submit">
                        <img src="../assets/icons/sendIcon.png" />
                    </button>
                </form>
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
import { toast } from 'vue3-toastify';
import axios from 'axios';
import config from '@/config';

export default defineComponent({
    name: "Chat",
    data() {
        return {
            userId: 0,
            chat: {} as IChat,
            message: '',
            messages: [] as IMessage[],
            loading: true,
            textArea: null as HTMLTextAreaElement | null
        }
    },
    created() {
        this.chat.id = Number(this.$route.params.id);
        this.userId = (JSON.parse(localStorage.getItem('user') ?? '{}') as IUser).id;
        this.textArea = this.$refs.messageInput as HTMLTextAreaElement;
        if (this.textArea) {
          this.textArea.addEventListener('input', this.handleInput);
        }
        this.loadChatData(this.chat.id);
        this.loadMessages();
    },
    methods: {
        getMessageClass(senderId: number): string {
            let isUsersMessage = this.userId == senderId;
            return isUsersMessage ? 'message-right' : 'message-left';
        },
        handleInput() {
            if (this.textArea) {
              this.textArea.style.height = 'auto';
              this.textArea.style.height = `${Math.min(this.textArea.scrollHeight, 100)}px`;
            }
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

            const messageInput = {
                senderId: this.userId,
                chatId: this.chat.id,
                message: this.message
            } as IMesssageInput;

            this.message = '';

            axios.post(`${config.apiUrl}/Message/send`, messageInput, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                }
            })
                .then((response) => {
                    const data = response.data as IResponse<IMessage>;

                    if (data.responseResult == 'Success' && data.value) {
                        this.messages.push(data.value);
                    }
                    else {
                        toast.error(data.responseResult);
                    }
                })
                .catch(error => {
                    toast.error('server error');
                });


        }
    }
});

</script>

<style>

    .header {
        height: 10%;
    }

    .chatView {
        display: grid;
        font-size: small;
        grid-template-rows: 60px auto 40px;
        grid-gap: 0;
        height: 100%;
    }

    .messages {
        overflow-y: scroll;
    }

    .messages li {
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
        display: grid;
        grid-template-columns: auto 30px;
    }

    textarea {
        columns: auto;
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

    .sender button {
        padding: 0px;
        background: none;
        border: none;
    }

</style>