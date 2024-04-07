<template>
    <div class="notification-list" v-if="!loading">
        <ul>
            <li class="notification-elem" v-for="notification in notifications" :key="notification.id">
                <div class="notification-card">
                    <div class="profile-image">
                        <img v-if="!isUserHaveAvatar(notification.id)" src="../assets/Images/Profile/Profile_default.png">
                        <img v-if="isUserHaveAvatar(notification.id)" :src="userAvatar(notification.notificationSenderId)">
                    </div>
                    <div style="padding: 5px;">
                        <div>
                            {{ notificationMessage(notification.id) }}
                        </div>
                        <div class="actions">
                            <div @click="handleFriendshipResponse(true, notification.id)">
                                <img src="../assets/icons/acceptfriendshipIcon.png">
                            </div>
                            <div @click="handleFriendshipResponse(false, notification.id)">
                                <img src="../assets/icons/rejectfriendshipIcon.png">
                            </div>
                        </div>
                    </div>
                </div>
            </li>
        </ul>
    </div>
    <div v-if="loading" style="display: flex; width: 100%; height: 100%; justify-content: center; align-items: center;">
        <VueSpinner :size="20"/>
    </div>
</template>

<script setup lang="ts">
import { VueSpinner } from 'vue3-spinners'
import type { INotificationModel, IResponse, IUser } from '@/api/types';
import config from '@/config';
import type { INotificationStore } from '@/store/notification.store';
import axios from 'axios';
import { computed, inject, onMounted, ref } from 'vue';
import { toast } from 'vue3-toastify';

const store = inject<INotificationStore>('notificationStore') as INotificationStore;
const loading = ref(true);
const notifications = computed<INotificationModel[]>(() => {
    return store.getFriendNotifications;
});

const userProfiles = ref<IUser[]>([]);

onMounted(async () => {
    await fetchUserProfiles();
    loading.value = false;
});

async function fetchUserProfiles() {
    notifications.value.forEach(notification => {
        let id = notification.notificationSenderId;
        axios.post(`${config.apiUrl}/User/getUser`, { id: id },{
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        }).then(response => {
            const data = response.data as IResponse<IUser>;

            if (data.responseResult == 'Success' && data.value){
                userProfiles.value.push(data.value);
            }
        })
    });
}

function isUserHaveAvatar(notificationId: number) : boolean{
    const id = notifications.value.find(x => x.id == notificationId)?.notificationSenderId;
    const user = userProfiles.value.find(x => x.id == id) as IUser;
    if (user && user.avatarBytes) {
        return true;
    }
    return false;
}

function userAvatar(id: number): string {
    const user = userProfiles.value.find(x => x.id == id) as IUser;
    return 'data:image/jpeg;base64,' + user.avatarBytes;
}

function notificationMessage(notificationId: number): string{
    const id = notifications.value.find(x => x.id == notificationId)?.notificationSenderId;
    const user = userProfiles.value.find(x => x.id == id) as IUser;
    return `${user.name} ${user.surname} offers you friendship`;
}

function handleFriendshipResponse(accept: boolean, id: number){
    const notification = notifications.value.find(x => x.id == id);
    const userId = notification?.notificationSenderId;
    const user = userProfiles.value.find(x => x.id == userId) as IUser;

    if (notification){
        axios.post(`${config.apiUrl}/User/createFriendship`, {
            userId: notification.userId,
            friendId: notification.notificationSenderId
        }).then(async (response) => {
            const data = response.data as IResponse<any>;
            
            if (data.responseResult == 'Success' && data){
                toast.success(`${user.name} ${user.surname} and You become friends`);
                await new Promise(resolve => setTimeout(resolve, 2000));
            }
        });
    }
}


</script>

<style>
.profile-image {
    display: flex;
    align-items: center;
    padding: 5px;
}

.profile-image img{
    width: 36px;
    height: 36px;
    border-radius: 18px;
}
.notification-card {
    display: flex;
    width: 100%;
    flex-direction: row;
    padding: 5px;
    border-radius: 10px;
}

.notification-card:hover {
    border-radius: 10px;
    background-color: #555;
}

.notification-list {
    display: flex;
    overflow-y: scroll;
    width: 100%;
    padding: 0px;
    margin: 0px;
}

.notification-list ul {
    width: 100%;
    padding: 0px;
    margin: 0px;
}

.actions {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
}

.notification-elem {
    margin: 5px;
}

.notification-list::-webkit-scrollbar { 
    width: 10px;
} 

.notification-list::-webkit-scrollbar-track { 
    background: #222222;
    border-radius: 5px;
} 
.notification-list::-webkit-scrollbar-thumb { 
    background: #292929;
    border-radius: 5px;
} 
.notification-list::-webkit-scrollbar-thumb:hover { 
    background: #555; 
} 
.notification-list::-webkit-scrollbar-button { 
    display: none; 
}


</style>