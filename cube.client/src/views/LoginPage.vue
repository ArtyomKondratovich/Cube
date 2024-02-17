<template>
    <div>
        <h2>Login</h2>
        <form @submit="handleSubmit">
            <div class="form-group">
                <label for="email">email</label>
                <input type="email" v-model="login.email" name="email" class="form-control" :class="{ 'is-invalid': submitted && !login.email }" />
                <div v-show="submitted && !login.email" class="invalid-feedback">email is required</div>
            </div>
            <div class="form-group">
                <label htmlFor="password">Password</label>
                <input type="password" v-model="login.password" name="password" class="form-control" :class="{ 'is-invalid': submitted && !login.password }" />
                <div v-show="submitted && !login.password" class="invalid-feedback">Password is required</div>
            </div>
            <div class="form-group">
                <button class="btn btn-primary" :disabled="loggingIn">Login</button>
            </div>
        </form>
    </div>
</template>

<script lang="ts">

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