<template>
    <div class="main-tools">
        <ul>
            <li>
                <div class="profile">
                </div>
            </li>
            <li>
                <router-link to="/home">Home</router-link>
            </li>
            <li>
                <router-link to="/messages">Messages</router-link>
            </li>
            <li>
                <router-link to="/notifications">Notifications</router-link>
            </li>
            <li>
                <router-link to="/friends">Friends</router-link>
            </li>
            <li>
                <router-link to="/settings">Settings</router-link>
            </li>
        </ul>
    </div>
 </template>
 
 <script lang="ts">
 import { defineComponent } from 'vue';
 
 export default defineComponent({
     name: 'Menu'
 });
 
 </script>
 
 <style>
     
 </style>