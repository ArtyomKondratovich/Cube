<template>
    <div class="main">
        <div>
            <Menu></Menu>
        </div>
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
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useChatStore } from '@/store/chats.store';
import type { 
    IChatLoad,
    IResponse 
} from '@/api/types';
import Menu from '@/components/Menu.vue';
import { toast } from 'vue3-toastify';

export default defineComponent({
    name: "Messages",
    components: {
        Menu
    },
    data() {
        return {
            loading: true,
            chats: [] as IChatLoad[]
        }
    },
    mounted() {
        const store = useChatStore();
        store.loadChats()
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
            this.$router.push({ name: 'Chat', params: { chatId: id } })
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

    .chats {
        border: 0.5px solid grey;
        border-radius: 15px;
        margin-left: 10px;
        width: 80%;
    }

    .chats div {
        width: 100%;
    }

    .chats ul {
        list-style: none;
        margin-top: 5px;
        padding-left: 5px;
        padding-right: 5px;
    }

    li {
        display: flex;
        align-items: center;
    }

    .chat {
        display: flex;
        border: 1px solid grey;
        border-radius: 15px;
        padding-left: 10px;
        padding-right: 10px;
        user-select: none;
        -moz-user-select: none;
        -webkit-user-select: none;
        -ms-user-select: none;
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

    .chat:hover{
        background-color: whitesmoke;
    }

</style>