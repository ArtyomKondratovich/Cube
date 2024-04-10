<template>
    <div class="friends">
        <div class="searchHeader">
            <div id="searchBar1">
                <input type="text" placeholder="Search" v-model="searchFriendText">
                <img src="../assets/icons/searchIcon.png">
            </div>
        </div>
        <div v-if="loadingFriends" style="display: flex; width: 100%; height: 100%; justify-content: center; align-items: center;">
            <VueSpinner :size="20"/>
        </div>
        <div v-if="!loadingFriends && friends.length == 0" style="display: flex; width: 100%; height: 100%; justify-content: center; align-items: center; font-size: small;" >
            Add some friends
        </div>
        <div v-if="!loadingFriends && friends.length > 0" class="friendsContainer">
            <ul>
                <li v-for="friend in filteredFriends" :key="friend.id">
                    <router-link :to="{ path: `/profile/${friend.id}`}">
                        <div class="userProfile">
                            <img v-if="friend.avatarBytes == null" src="../assets/Images/Profile/Profile_default.png">
                            <img v-if="friend.avatarBytes != null" :src="userAvatar(friend.avatarBytes)">
                            <div class="userName"> {{ friend.name }} {{ friend.surname }} </div>
                        </div>
                    </router-link>
                </li>
            </ul>
        </div>
    </div>
    <div class="search">
        <div style="display: flex; width: 100%; height: 38px; align-items: center; justify-content: center; border-bottom: 1px solid #363738;">
            All existing users
        </div>
        <div v-if="loadingAllUsers" style="display: flex; width: 100%; height: 100%; justify-content: center; align-items: center;">
            <VueSpinner :size="20"/>
        </div>
        <div v-if="!loadingAllUsers" class="usersContainer">
            <ul>
                <li v-for="user in searchUsers" :key="user.id">
                    <router-link :to="{ path: `/profile/${user.id}`}">
                        <div class="userProfile">
                            <img v-if="user.avatarBytes == null" src="../assets/Images/Profile/Profile_default.png">
                            <img v-if="user.avatarBytes != null" :src="userAvatar(user.avatarBytes)">
                            <div class="userName"> {{ user.name }} {{ user.surname }} </div>
                        </div>
                    </router-link>
                </li>
            </ul>
        </div>
    </div>
</template>

<script setup lang="ts">

import { VueSpinner } from 'vue3-spinners'
import { ref, onMounted, computed } from 'vue'
import type { IResponse, IUser } from '@/api/types';
import axios from 'axios';
import config from '@/config';
import router from '@/helpers/router';
    
const user = ref(JSON.parse(localStorage.getItem('user') ?? '{}') as IUser);
const friends = ref([] as IUser[])
const allUsers = ref([] as IUser[])
const loadingFriends = ref(true);
const loadingAllUsers = ref(true);
const searchFriendText = ref('');

const searchUsers = computed(() => {
    let users = allUsers.value.filter(x => x.id != user.value.id && friends.value.findIndex(y => y.id == x.id) == -1);
    return users;
});

const filteredFriends = computed(() => {
  if (!searchFriendText.value) {
    return friends.value;
  }
  const searchQuery = searchFriendText.value.toLowerCase();
  return friends.value.filter(friend => (friend.name + " " + friend.surname).toLowerCase().includes(searchQuery));
});

function fetchData(): void {
    axios.post(`${config.apiUrl}/User/getUserFriends`, { Id: user.value.id }, { 
    headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        'Content-Type': 'application/json'
    }}).then(async (response) => {
        const data = response.data as IResponse<IUser[]>;
        if (data.responseResult == 'Success' && data.value){
            friends.value = data.value;
            friends.value = friends.value.filter(x => x.id != user.value.id);
        }
        else {
            console.log(data.responseResult);
        }
        loadingFriends.value = false;
    }).catch(error => console.log(error));

axios.post(`${config.apiUrl}/User/getAllUsers`,{}, { 
    headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        'Content-Type': 'application/json'
    }}).then(async (response) => {
        const data = response.data as IResponse<IUser[]>;
        if (data.responseResult == 'Success' && data.value){
            allUsers.value = data.value;
        }
        else {
            console.log(data.responseResult);
        }
        loadingAllUsers.value = false;
    }).catch(error => {
        if (error.response && error.response.status == 401){
            console.log(error);
            router.push({ path: '/login'})
        }
    });
}

function userAvatar(bytes: []): string {
    return 'data:image/jpeg;base64,' + bytes;
}

onMounted(() => {
    fetchData();
})
</script>

<style>

.friends{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    flex-basis: 30%;
    border-right: 1px solid #363738;
}

.userProfile {
    display: flex;
    width: 100%;
    background-color: #292929;
    border-radius: 10px;
}

.userProfile img {
    width: 36px;
    height: 36px;
    border-radius: 18px;
}

.searchHeader {
    display: flex;
    height: 24px;
    justify-content: center;
    width: 100%;
    padding: 5px;
}

.usersContainer {
    display: flex;
    width: 100%;
}

.usersContainer a {
    width: 100%;
}

.search {
    display: flex;
    flex-basis: 70%;
    flex-direction: column;
}

.search ul {
    width: 100%;
    padding: 0px;
    margin: 0px;
}

.search ul li {
    margin: 0px;
    padding: 5px;
}

#searchBar1 {
    display: flex;
    border-radius: 10px;
    background-color: #292929;
}

#searchBar1 input {
    color: white;
    padding-left: 5px;
    background-color: #292929;
    border: none;
    outline: none;
    border-top-left-radius: 10px;
    border-bottom-left-radius: 10px;
}

#findFriends {
    border: none;
    border-radius: 10px;
    background-color: #292929;
}

.friendsContainer {
    display: flex;
    width: 100%;

}

.friendsContainer ul{
    width: 100%;
    margin: 0px;
    padding: 0px;
}

.friendsContainer li{
    padding: 5px;
}

.friendsContainer a{
    width: 100%;
    
}

</style>