<template>
    <div class="register-form">
        <h2>SignUp</h2>
        <form @submit.prevent="handleSubmit">
            <ul>
                <li>
                    <input type="file" ref="imageInput" accept=".png, .jpeg, .jpg" @change="previewAvatar"/>
                </li>
                <li>
                    <input type="text" v-model="user.name" name="name" placeholder="name" class="form-control" :class="{ 'is-invalid': submitted && !user.name }" />
                    <div v-show="submitted && !user.name" class="invalid-feedback">name is required</div>
                </li>
                <li>
                    <input type="text" v-model="user.surname" name="surname" placeholder="surname" class="form-control" :class="{ 'is-invalid': submitted && !user.surname }" />
                    <div v-show="submitted && !user.surname" class="invalid-feedback">surname is required</div>
                </li>
                <li>
                    <input type="date" v-model="user.dateOfBirth" name="dateOfBirth" placeholder="birthday" class="form-control"/>
                </li>
                <li>
                    <input type="email" v-model="user.email" name="email" placeholder="email" class="form-control" :class="{ 'is-invalid': submitted && !user.email }" />
                    <div v-show="submitted && !user.email" class="invalid-feedback">email is required</div>
                </li>
                <li>
                    <input type="password" v-model="user.password" name="password" placeholder="password" class="form-control" :class="{ 'is-invalid': submitted && !user.password }" />
                    <div v-show="submitted && !user.password" class="invalid-feedback">Password is required</div>
                </li>
                <li>
                    <input type="password" v-model="confirmPassword" name="password" placeholder="confirm password" class="form-control" :class="{ 'is-invalid': submitted && user.password == confirmPassword }" />
                    <div v-show="submitted && user.password == confirmPassword" class="invalid-feedback">Passwords doesn match</div>
                </li>
                <li>
                    <button class="btn btn-primary" :disabled="!isLoggedIn">SignUp</button>
                </li>
            </ul>
        </form>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useAuthStore } from '@/store/auth.store';
import type { IAuth, IRegisterInput, IResponse } from '@/api/types';
import { preview } from 'vite';
import axios from 'axios';
import config from '@/config';
import { toast } from 'vue3-toastify';
import router from '@/helpers/router';

const submitted = ref(false);
const user = ref<IRegisterInput>({} as IRegisterInput);
const confirmPassword = ref('');
const imageInput = ref<HTMLInputElement | null>(null);
const imagePreview = ref<string | null>(null);

function isLoggedIn(): boolean {
    const store = useAuthStore();
    return store.isLoggedIn;
}

async function handleSubmit(){
    const selectedImage = imageInput.value?.files?.[0];

    if (selectedImage)
    {
        let formData = new FormData();
        formData.append('name', user.value.name);
        formData.append('surname', user.value.surname);
        formData.append('dateOfBirth', user.value.dateOfBirth ? user.value.dateOfBirth.toString() : "");
        formData.append('email', user.value.email);
        formData.append('password', user.value.password)
        formData.append('file', selectedImage);

        axios.post(`${config.apiUrl}/User/register`, formData, {
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

        const store = useAuthStore();
        user.value.file = selectedImage;
        await store.register(user.value);
    }
}

function previewAvatar(): void {
    const selectedImage = imageInput.value?.files?.[0];

    if (selectedImage) {
        imagePreview.value = selectedImage.name
    }
}
</script>

<style>
    .register-form{
        width: 20%;
      margin-top: 10%;
      margin-left: 40%;
      margin-right: 40%;
    }
    .form-group {
        display: block;
        align-items: center;
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