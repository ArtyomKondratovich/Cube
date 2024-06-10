<template>
    <div class="register-view">
        <div class="sendToken-form" v-if="!submit && !emailConfirmed">
            <h2>Verify email</h2>
            <form>
              <div style="display: flex; align-items: center; margin-top: 5px;">
                <img src="../assets/icons/emailIcon.png">
                <input type="email" title="email" v-model="email" placeholder="example@gmail.com" />
              </div>
              <div v-on:click.prevent="sendToken()" 
              style="
                display: flex; 
                justify-content: center;
                padding: 5px;
                border-radius: 20px;
                background-color: #222222;">
                
                <img src="../assets/icons/nextStepIcon.png" width="24px" height="24px">
              </div>
            </form>
        </div>
        <div class="confirmToken-form" v-if="submit && !emailConfirmed"> 
            <span>Verification token sent by email (token lifetime 1min)</span>
            <div>
                <form>
                    <div style="display: flex; align-items: center; margin-top: 5px;">
                        <img src="../assets/icons/passwordIcon.png">
                        <input type="password" title="token" v-model="token" placeholder="Enter token..."/>
                    </div>
                    <div style="display: flex; justify-content: center; margin-top: 5px; width: 50%;">
                      <button type="submit" v-on:click.prevent = "confirmToken()" v-bind:disabled="submitted" class="btn">confirm</button>
                    </div>
                </form>
                <div>
                    <div>
                        <spna>resend token in {{ timer.minutes}}:{{ timer.seconds }}</spna>
                    </div>
                    <div v-if="timer.isExpired">
                        <button v-on:click.prevent="sendToken()"></button>
                    </div>
                </div>
            </div>
        </div>
        <div class="register-form" v-if="emailConfirmed">
            <div style="display: flex; flex-grow: 1; flex-direction: column;">
                <h2>SignUp</h2>
                <form>
                    <div>
                      <input type="file" ref="avatarInput" accept=".png, .jpeg, .jpg" @change="previewAvatar"/>
                    </div>
                    <div>
                        <img src="../assets/icons/unknownUsericon.png">
                        <input type="text" v-model="user.name" name="name" placeholder="name" class="form-control" :class="{ 'is-invalid': submitted && !user.name }" />
                    </div>
                    <div>
                        <img src="../assets/icons/unknownUsericon.png">
                        <input type="text" v-model="user.surname" name="surname" placeholder="surname" class="form-control" :class="{ 'is-invalid': submitted && !user.surname }" />
                    </div>
                    <div>
                        <img src="../assets/icons/birthdayIcon.png">
                        <input type="date" v-model="user.dateOfBirth" name="dateOfBirth" placeholder="birthday" class="form-control" />
                    </div>
                    <div>
                        <img src="../assets/icons/emailIcon.png">
                        <input type="email" v-model="user.email" name="email" readonly/>
                    </div>
                    <div>
                        <img src="../assets/icons/passwordIcon.png">
                      <input type="password" v-model="user.password" name="password" placeholder="password" class="form-control" :class="{ 'is-invalid': submitted && !user.password }" />
                    </div>
                    <div>
                        <img src="../assets/icons/passwordIcon.png">
                      <input type="password" v-model="confirmPassword" name="password" placeholder="confirm password" class="form-control" :class="{ 'is-invalid': submitted && user.password == confirmPassword }" />
                    </div>
                    <div style="display: flex; justify-content: center; margin-top: 5px; width: 50%;">
                      <button type="submit" v-on:click.prevent = "handleSubmit" :disabled="submitted">SignUp</button>
                    </div>
                </form>
            </div>
                <img v-if="avatarPreviewUrl" :src="avatarPreviewUrl" class="preview-avatar">
                <img v-if="!avatarPreviewUrl" src="../assets/Images/Profile/Profile_default.png" class="preview-avatar">
        </div>
    </div>
</template>

<script setup lang="ts">

import config from '@/config';
import axios from 'axios';
import { ref } from 'vue'
import { useTimer } from 'vue-timer-hook'
import type { IAuth, IRegisterInput, IResponse } from '@/api/types';
import { toast } from 'vue3-toastify';
import router from '@/helpers/router';

const submit = ref(false);
const email = ref('');
const token = ref('');
const emailConfirmed = ref(false);
const timer = ref(useTimer());

const submitted = ref(false);
const user = ref<IRegisterInput>({} as IRegisterInput);
const confirmPassword = ref('');
const avatarInput = ref<HTMLInputElement | null>(null);
const avatarPreviewUrl = ref<string | null>(null);


function sendToken(){
    submit.value = true;
    axios.post(`${config.apiUrl}/Email/send`, { Email: email.value }, {
        headers: {
            'Content-Type': 'application/json'
        }
    });

    var now = new Date();
    now.setSeconds(now.getSeconds() + 60); //minute timer
    timer.value = useTimer(now);
    timer.value.start();
}

function confirmToken() {
    user.value.email = email.value;
    emailConfirmed.value = true;
}

async function handleSubmit(): Promise<void>{
    if (confirmPassword.value != user.value.password){
        toast.warning('Your passwords doesn\'t match');
        user.value.password = '';
        confirmPassword.value = '';
        submitted.value = false;
        return;
    }

    submitted.value = true;
    const selectedImage = avatarInput.value?.files?.[0];

    let formData = new FormData();
    formData.append('name', user.value.name);
    formData.append('surname', user.value.surname);
    formData.append('dateOfBirth', user.value.dateOfBirth ? user.value.dateOfBirth.toString() : "");
    formData.append('email', user.value.email);
    formData.append('password', user.value.password)

    if (selectedImage){
        formData.append('file', selectedImage);
    }

    await axios.post(`${config.apiUrl}/User/register`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }).then(async (response) => {
          const data = response.data as IResponse<IAuth>;
          if (data.responseResult == 'Success')
          {
            toast.success('You register successfully!');
            await new Promise(resolve => setTimeout(resolve, 2000));
            router.push('/login');
          }
          else
          {
            toast.error(data.responseResult);
            await new Promise(resolve => setTimeout(resolve, 2000));
            router.push('/register');
          }
        }).catch(async error => {
            //handling error
            toast.error(error);
            await new Promise(resolve => setTimeout(resolve, 2000));
            router.push('/register');
        });

    user.value = {} as IRegisterInput;
    confirmPassword.value = '';
    submitted.value = false;
}

function previewAvatar() {
  const input = avatarInput.value;
  if (input && input.files && input.files[0]) {
    const reader = new FileReader();
    reader.onload = (e) => {
      avatarPreviewUrl.value = e.target?.result?.toString() || null;
    };
    reader.readAsDataURL(input.files[0]);
  }
}


</script>

<style>

    .register-view{
        display: flex;
        width: 100%;
        height: 100%;
        align-items: center;
        justify-content: center;
    }

    .register-block{
        display: flex;
        width: 100%;
        height: 100%;
        align-items: center;
        justify-content: center;
    }

    .register-form {
        display: flex;
        background-color: #222222;
        border-radius: 10px;
        width: 50%;
        max-width: 600px;
        box-shadow: 0 0 10px #222222;
        height: 40%;
        max-height: 400px;
    }

    .register-form form {
        display: flex;
        align-items: center;
        width: 100%;
    }

    .register-form form div {
        display: flex;
        justify-content: flex-start; 
        margin-top: 5px;
        width: 50%;
    }

    h2 {
      text-align: center;
    }

    .preview-avatar {
        display: flex;
        margin: 15px;
        width: 200px;
        height: 200px;
        border-radius: 100px;
    }
</style>