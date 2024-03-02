<template>
    <div>
        <h2>Login</h2>
        <form @submit.prevent="handleLogin">
            <div class="form-group">
                <label for="email">email</label>
                <input type="email" v-model="user.email" name="email" class="form-control" :class="{ 'is-invalid': !user.email }" />
                <div v-show="!user.email" class="invalid-feedback">email is required</div>
            </div>
            <div class="form-group">
                <label htmlFor="password">Password</label>
                <input type="password" v-model="user.password" name="password" class="form-control" :class="{ 'is-invalid': !user.password }" />
                <div v-show="!user.password" class="invalid-feedback">Password is required</div>
            </div>
            <div class="form-group">
                <button class="btn btn-primary" :disabled="isLoggedIn">Login</button>
            </div>
        </form>
    </div>
</template>

<script lang="ts">

import { defineComponent, type PropType } from "vue"

export default defineComponent({
  
})


</script>

<style>
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