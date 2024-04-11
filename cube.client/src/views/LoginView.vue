<template>
  <div class="login-form">
    <div class="log-block">
      <h2>Login</h2>
      <form>
        <div style="display: flex; align-items: center; margin-top: 5px;">
          <img src="../assets/icons/emailIcon.png">
          <input type="email" title="email" v-model="email" placeholder="example@gmail.com" />
        </div>
        <div style="display: flex; align-items: center; margin-top: 5px;">
          <img src="../assets/icons/passwordIcon.png">
          <input type="password" title="username" v-model="password" placeholder="password" />
        </div>
        <div style="display: flex; justify-content: center; margin-top: 5px; width: 50%;">
          <button type="submit" v-on:click.prevent = "onSubmit" v-bind:disabled="submitted" class="btn">Login</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { IAuth, IResponse } from '@/api/types';
import { type IAuthStore } from '../store/auth.store';
import {  ref, inject } from 'vue';
import { toast } from 'vue3-toastify';
import router from '@/helpers/router';
import axios from 'axios';
import config from '@/config';
import type { INotificationStore } from '@/store/notification.store';

const submitted = ref(false);
const email = ref('');
const password = ref('');
const authStore = inject<IAuthStore>('authStore');
const notificationStore = inject<INotificationStore>('notificationStore');

function onSubmit() {
  if (authStore && notificationStore){
    submitted.value = true;
    axios.post(`${config.apiUrl}/User/login`, {
      email: email.value, 
      password: password.value
    }, {
      headers: {
          'Content-Type': 'application/json'
      }
    }).then(async (response) => {
      const data = response.data as IResponse<IAuth>;
      
      if (data.responseResult == 'Success' &&  data.value)
      {
        toast.success('Authentication was successful');
        await new Promise(resolve => setTimeout(resolve, 2000));
        localStorage.setItem('token', data.value.token);
        localStorage.setItem('user', JSON.stringify(data.value.user));
        authStore.$state.user = data.value.user;
        authStore.$state.token = data.value.token;
        notificationStore.login(data.value.user.id);
        router.push({ path: '/feed' });
      }
      else{
        toast.error(data.responseResult);
        router.push({ path: '/login' });
      }
    })
    .catch(error => {
        //handling error
        toast.error(error);
        router.push({ path: '/login' });
    }).finally(() => {
      email.value = '';
      password.value = '';
      submitted.value = false;
    });
  }
}
</script>

<style>
    .login-form {
      display: flex;
      justify-content: center;
      align-items: center;
      width: 100%;
      height: 100%;
    }

    .log-block {
      display: flex;
      flex-direction: column;
      justify-content: center;
      background-color: #222222;
      border-radius: 10px;
      width: 50%;
      max-width: 500px;
      box-shadow: 0 0 10px #222222;
      height: 30%;
    }

    form {
      display: flex;
      align-items: center;
      flex-direction: column;
    }

    form input{
      color: white;
      background-color: #222222;
      border: none;
      appearance: none;
      -webkit-appearance: none;
      -moz-appearance: none;
      -ms-appearance: none;
      outline: none;
    }

    form button {
      width: 40%;
      border-radius: 10px;
      border: none;
      height: 30px;
      background-color: #363738;
      color: white;
    }

    form button:active {
      background-color: #222222;
    }

    form button:disabled {
      background-color: #222222;
      opacity: 0.5;
      cursor: not-allowed;
    }
    
</style>