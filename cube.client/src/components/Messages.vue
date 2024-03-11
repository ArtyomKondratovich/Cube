<template>
    <div class="chats">
            <div v-if="loading">
                <p>Loading chats...</p>
            </div>
            <div v-if="!loading">
                <ul>
                    <li v-for="chat in chats" :key="chat.id">
                        <div class="chat" @click="stepIntoChat(chat.id)">
                            <img src="../assets/icons/savedMessages.png" class="image">
                            <h3>{{chat.title}}</h3>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import type { 
    IChatLoad,
    IResponse, 
    IUser
} from '@/api/types';
import { toast } from 'vue3-toastify';
import axios from 'axios';
import config from '@/config';

export default defineComponent({
    name: "Messages",
    data() {
        return {
            loading: true,
            chats: [] as IChatLoad[]
        }
    },
    created() {
        let userId = (JSON.parse(localStorage.getItem('user') ?? '{}') as IUser).id;

        axios.post(`${config.apiUrl}/Chat/getUserChats`, { Id: userId }, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        })
        .then(async (response) => {
                const data = response.data as IResponse<IChatLoad[]>;
                if (data.responseResult == 'Success' && data.value){
                    this.chats = data.value;
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
        stepIntoChat(id: number): void{
            this.$router.push(`/home/chat/${id}`)
        }
    }
});

</script>

<style>
    .chats div {
        width: 100%;
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

    h3 {
        display: inline-block;
        vertical-align: middle;
        padding-left: 10px;
        cursor: default;
    }

    /* .chat:hover{
        background-color: whitesmoke;
    } */

</style>