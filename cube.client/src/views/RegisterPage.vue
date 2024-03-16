<template>
    <div class="register-form">
        <h2>SignUp</h2>
        <form @submit.prevent="handleSubmit">
            <ul>
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

<script lang="ts">
import { defineComponent } from 'vue';
import { useAuthStore } from '@/store/auth.store';

    export default defineComponent({
        name: 'RegisterView',
        components: {},
        data() {
            return {
                submitted: false,
                user: {
                    name: '',
                    surname: '',
                    dateOfBirth: null,
                    email: '',
                    password: ''
                },
                confirmPassword: ''
            }
        },
        methods: {
            isLoggedIn(): boolean {
                const store = useAuthStore();
                return store.isLoggedIn;
            },
            async handleSubmit(){
                const store = useAuthStore();
                await store.register(this.user);
            }

        }
    })
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