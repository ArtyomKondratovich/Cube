<template>
    <div class="profileView">
        <div v-if="loading" id="loading">
            <VueSpinner size="20"/>
        </div>
        <div v-if="!loading" class="profileHeader">
            <div class="colored-backgroud"></div>
            <div class="profileHeader-content">
                <div id="img-block">
                    <img id="profileImage" src="../assets/Images/Profile/Profile_default.png">
                </div>
                <div id="info-block">
                    {{ "Artyom" + " " + "Kondratovich" }}
                </div>
                <div id="actions-block">
                    <div id="add-friend" v-if="!isUserProfile" @click="">
                        <img src="../assets/icons/addFriendIcon.png" width="20px" height="20px">
                    </div>
                    <div id="edit-profile" @click="">
                        <img src="../assets/icons/editIcon.png">
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</template>

<script setup lang="ts">
import type { IResponse, IUser } from '@/api/types';
import { VueSpinner } from 'vue3-spinners';
import config from '@/config';
import { useAuthStore } from '@/store/auth.store';
import axios from 'axios';
import { ref } from 'vue';

    const props = defineProps({
        userId: {
            type: Number,
            required: true
        }
    });

    const store = useAuthStore();
    const isUserProfile = ref(store.$state.user?.id != props.userId);
    const loading = ref(false);
    const user = ref<IUser | null>(null);

    axios.post(`${config.apiUrl}/User/GetUser}`, { id: props.userId },
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

    #loading {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
    }


</style>