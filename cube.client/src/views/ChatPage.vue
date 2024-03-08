<template>
    <div class="main">
        <div>
            <Menu></Menu>
        </div>
        <div class="messages">
            <div v-if="loading">
                <p>Loading messages...</p>
            </div>
            <div v-if="!loading">
                <ul>
                    <li v-for="message in messages" :key="message.id">
                        <div class="message">
                            <h3>{{ message.message }}</h3>
                            <h3>{{ message.createdDate }}</h3>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useChatStore } from '@/store/chats.store';
import type { 
    IChat,
    IChatLoad,
    IMessage,
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
            chatId: 0,
            loading: true,
            participants: [] as IUser[],
            messages: [] as IMessage[]
        }
    },
    mounted() {
        this.chatId = Number(this.$route.params.chatId);

        let id = this.chatId;

        axios.post(`${config.apiUrl}/Chat/Get`, { id }, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        })
        .then(async (response) => {
            const data = response.data as IResponse<IChat>;

            if (data.responseResult == 'Success' && data.value){
                this.messages = data.value.messages;
                this.participants = data.value.participants;
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

    .messages {
        border: 0.5px solid grey;
        border-radius: 15px;
        margin-left: 10px;
        width: 80%;
    }

</style>