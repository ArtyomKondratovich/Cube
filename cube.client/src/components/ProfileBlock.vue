<template>
    <div class="profileView">
        <div v-if="loading" id="loading">
            <VueSpinner size="20"/>
        </div>
        <div v-if="!loading" class="profileHeader">
            <div class="colored-backgroud"></div>
            <div class="profileHeader-content">
                <div id="img-block">
                    <img v-if="user?.avatarBytes != null && !loading" id="profileImage" :src="userAvatar()">
                    <img v-if="user?.avatarBytes == null" id="profileImage" src="../assets/Images/Profile/Profile_default.png">
                </div>
                <div id="info-block">
                    {{ user?.name + " " + user?.surname }}
                </div>
                <div id="actions-block">
                    <div id="send-message" v-if="!isUserProfile" @click="sendMessage">
                        <img src="../assets/icons/sendMessageIcon.png">
                    </div>
                    <div id="add-friend" v-if="!isUserProfile" @click="friendRequest">
                        <img src="../assets/icons/addFriendIcon.png">
                    </div>
                    <div id="edit-profile" v-if="isUserProfile" @click="">
                        <img src="../assets/icons/editIcon.png">
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</template>

<script setup lang="ts">
import type { IChat, IResponse, IUser } from '@/api/types';
import { VueSpinner } from 'vue3-spinners';
import config from '@/config';
import { type IAuthStore } from '@/store/auth.store';
import axios from 'axios';
import { ref, onMounted, watch, toRef, inject } from 'vue';
import getUserIdFromLocalStorage from '@/helpers/getFromLocalStorage';
import { toast } from 'vue3-toastify';
import router from '@/helpers/router';

const props = defineProps({
    userId: {
        type: Number,
        required: true
    }
});
const store = inject<IAuthStore>('authStore') as IAuthStore;
const isUserProfile = ref(store.$state.user?.id == props.userId);
const loading = ref(true);
const user = ref<IUser>({} as IUser);

async function updateInfo(id: number) {
    await fetchUser(id);
    isUserProfile.value = store.$state.user?.id == props.userId;
}
function userAvatar(): string {
    if (user.value){
        return 'data:image/jpeg;base64,' + user.value.avatarBytes;
    }
    return '';
}
async function fetchUser(id: number) {
    axios.post(`${config.apiUrl}/User/getUser`, { id: id },
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        }
    })
    .then(async (response) => {
        const data = response.data as IResponse<IUser>;
        if (data.responseResult == 'Success' && data.value) {
            user.value = data.value;
            loading.value = false;
        }
    })
}

function friendRequest() {
    toast.success(`Send friend request to ${user.value?.name} ${user.value?.surname}`);

    axios.post(`${config.apiUrl}/Notification/create`, {
        notificationSenderId: getUserIdFromLocalStorage(),
        userIds: [
          props.userId
        ],
        isReaded: false,
        type: "FriendRequest",
        accepted: false
    },
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        }
    }).then(response => {
        console.log(response.data);
    });
}

async function sendMessage() {
    let chats = [] as IChat[];

    await axios.post(`${ config.apiUrl }/Chat/getUserChats`, { id: store.$state.user?.id }, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        }
    }).then(response => {
        const data = response.data as IResponse<IChat[]>;

        if (data.responseResult == 'Success' && data.value)
        {
            chats = data.value;
        }
        
    });

    let chat = chats.find(x => x.type == 'Private' && x.users.find(x => x.id == props.userId));

    if (chat) {
        router.push({ path: `/messanger/chat/${chat.id}`});
    }
    else {
        await axios.post(`${config.apiUrl}/Chat/create`, {
            title: 'PrivateChat',
            type: 'Private',
            patricipantsIds: [
                props.userId,
                store.$state.user.id
            ]
        }, { 
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        }).then(response => {
            const data = response.data as IResponse<IChat>;

            if (data.responseResult == 'Success' && data.value){
                router.push({ path: `/messanger/chat/${data.value.id}`});
            }
        })
    }

}

onMounted(async () => {
    await updateInfo(props.userId);
});
watch(toRef(props, 'userId'), async (newUserId) => {
    await updateInfo(newUserId);
});
</script>

<style>
    .profileView {
        display: flex;
        width: 100%;
    }

    #profileImage {
        width: 128px;
        height: 128px;
        border-radius: 64px;
    }

    .profileHeader {
        display: flex;
        position: relative;
        margin-top: 10px;
        height: 138px;
        width: 100%;
    }

    .profileHeader-content {
        position: relative;
        width: 100%;
        padding-bottom: 10px;
        padding-left: 20px;
        padding-right: 20px;
        z-index: 2;
        display: flex;
        align-items: center;
    }

    .colored-backgroud {
        display: flex;
        position: absolute;
        z-index: 1;
        border-top-left-radius: 10px;
        border-top-right-radius: 10px;
        top: 50%;
        left: 0;
        width: 100%;
        height: 50%;
        background-color: #2A2F33;
    }

    #img-block {
        flex: 0 0 auto;
    }

    #info-block {
        flex-grow: 1;
        align-self: flex-end;
    }

    #actions-block {
        display: flex;
        flex: 0 0 auto;
        margin-left: auto;
        align-self: flex-end;
    }

    #actions-block div{
        margin-left: 5px;
        
    }


    #edit-profile {
        display: flex;
        width: 24px;
        height: 24px;
        cursor: pointer;
        align-content: center;
    }

    #add-friend {
        display: flex;
        align-items: center;
        width: 24px;
        height: 24px;
        cursor: pointer;
    }

    #send-message {
        display: flex;
        align-items: center;
        width: 24px;
        height: 24px;
        cursor: pointer;
    }

    #loading {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
    }


</style>