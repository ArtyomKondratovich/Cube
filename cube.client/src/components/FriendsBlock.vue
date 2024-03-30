<template>
    <div class="friends">
        <div class="searchHeader">
            <div id="searchBar1">
                <input type="text" placeholder="Search">
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

        </div>
    </div>
    <div class="search">
        <div v-if="loadingAllUsers" style="display: flex; width: 100%; height: 100%; justify-content: center; align-items: center;">
            <VueSpinner :size="20"/>
        </div>
        <div v-if="!loadingAllUsers" class="usersContainer">
            <ul>
                <li v-for="user in allUsers" :key="user.id">
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
import { ref, computed, onMounted } from 'vue'
import type { IResponse, IUser } from '@/api/types';
import axios from 'axios';
import config from '@/config';
import router from '@/helpers/router';
    
const user = ref(JSON.parse(localStorage.getItem('user') ?? '{}') as IUser);
const friends = ref([] as IUser[])
const allUsers = ref([] as IUser[])
const loadingFriends = ref(true);
const loadingAllUsers = ref(true);

function fetchData(): void {
    axios.post(`${config.apiUrl}/User/getUserFriends`, { Id: user.value.id }, { 
    headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        'Content-Type': 'application/json'
    }}).then(async (response) => {
        const data = response.data as IResponse<IUser[]>;
        if (data.responseResult == 'Success' && data.value){
            friends.value = data.value;
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
            allUsers.value = allUsers.value.filter((x) => x.id != user.value.id);
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

</style>