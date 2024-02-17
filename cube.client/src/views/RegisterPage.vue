<template>
    <div>
        <h2>Login</h2>
        <form @submit.prevent="handleSubmit">
            <div class="form-group">
                <label for="name">name</label>
                <input type="text" v-model="register.Name" name="name" class="form-control" :class="{ 'is-invalid': submitted && !register.Name }" />
                <div v-show="submitted && !register.Name" class="invalid-feedback">name is required</div>
            </div>
            <div class="form-group">
                <label for="surname">surname</label>
                <input type="text" v-model="register.Surname" name="surname" class="form-control" :class="{ 'is-invalid': submitted && !register.Surname }" />
                <div v-show="submitted && !register.Surname" class="invalid-feedback">surname is required</div>
            </div>
            <div class="form-group">
                <label for="dateOfBirth">dateOfBirth</label>
                <input type="date" v-model="register.DateOfBirth" name="dateOfBirth" class="form-control"/>
            </div>
            <div class="form-group">
                <label for="email">email</label>
                <input type="email" v-model="register.Email" name="email" class="form-control" :class="{ 'is-invalid': submitted && !register.Email }" />
                <div v-show="submitted && !register.Email" class="invalid-feedback">email is required</div>
            </div>
            <div class="form-group">
                <label htmlFor="password">Password</label>
                <input type="password" v-model="register.Password" name="password" class="form-control" :class="{ 'is-invalid': submitted && !register.Password }" />
                <div v-show="submitted && !register.Password" class="invalid-feedback">Password is required</div>
            </div>
            <div class="form-group">
                <button class="btn btn-primary" :disabled="loggingIn">Login</button>
            </div>
        </form>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { RegisterDto } from '../models/registerDto'
import { useAuthStore } from '../stores';
import { userService } from '../services';

export default defineComponent({
    data() {
        return {
            auth: useAuthStore(),
            register: new RegisterDto(),
            submitted: false
        }
    },
    computed: {
        loggingIn() {
            return this.auth.isAuthenticate;
        }
    },
    created() {
        // reset login status
        userService.logout();
    },
    methods: {
        handleSubmit(): void {
            this.submitted = true;
            const registerDto = this.register;
            const { dispatch } = this.$store;
            if (registerDto.Email && registerDto.Password) {
                dispatch('authentication/register', { registerDto });
            }
        }
    }
    });
</script>

<style scoped>
    .form-signin {
        max-width: 330px;
        padding: 1rem;
    }

        .form-signin .form-floating:focus-within {
            z-index: 2;
        }

        .form-signin input[type="email"] {
            margin-bottom: -1px;
            border-bottom-right-radius: 0;
            border-bottom-left-radius: 0;
        }

        .form-signin input[type="password"] {
            margin-bottom: 10px;
            border-top-left-radius: 0;
            border-top-right-radius: 0;
        }
</style>