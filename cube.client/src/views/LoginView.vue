<template>
  <div class="login-form">
    <div class="log-block">
      <h2>SignIn</h2>
      <form>
        <ul>
          <li>
            <input type="email" title="email" v-model="email" placeholder="example@gmail.com" />
          </li>
          <li>
            <input type="password" title="username" v-model="password" placeholder="password" />
          </li>
          <li>
            <button type="submit" v-on:click.prevent = "onSubmit" v-bind:disabled="submitted" class="btn">Login</button>
          </li>
        </ul>
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
      width: 20%;
      margin-top: 10%;
      margin-left: 40%;
      margin-right: 40%;
    }
    form li{
      display: block;
      align-items: center;
    }
    form ul{
      text-align: center;
    }
    h2 {
      text-align: center;
    }
    
</style>