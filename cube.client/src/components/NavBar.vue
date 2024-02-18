<script setup>
    import authService from '@/services/authService'

</script>

<template>
    <nav class="navbar navbar-expand-md navbar-dark bg-dark mb-4">
        <div class="container-fluid">
            <div class="collapse navbar-collapse" id="navbarCollapse">
                <ul class="navbar-nav me-auto mb-2 mb-md-0">
                    <li class="nav-item">
                        <router-link to="/" class="nav-link active" aria-current="page">Home</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link to="/login" class="nav-link">Login</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link to="/register" class="nav-link disabled" aria-disabled="true">Register</router-link>
                    </li>
                    <li>
                        <button @click="authService.logout()">Logout</button>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</template>

<style>

</style>