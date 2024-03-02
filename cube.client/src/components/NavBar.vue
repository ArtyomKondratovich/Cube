<script setup lang="ts">
    import { useAuthStore } from '@/stores/authStore'
    import { useMutation } from 'vue-query';
    import { createToast } from 'mosha-vue-toastify'

    import authService from '@/services/authService';
    import router from '@/helpers/router';

    const authStore = useAuthStore();

    const { mutate: logoutUser } = useMutation(async () => authService.logout(), {
        onSuccess: () => {
            authStore.setAuthUser(null);
            router.push('/login')
        },
        onError: (error) => {
            if (Array.isArray((error as any).response.data.error)) {
              (error as any).response.data.error.forEach((el: any) =>
                createToast(el.message, {
                  position: 'top-right',
                  type: 'warning',
                })
              );
            } else {
              createToast((error as any).response.data.message, {
                position: 'top-right',
                type: 'danger',
              });
            }
        },
    });

    const handleLogout = () => {
        logoutUser();
    };

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
                        <button @click="handleLogout()">Logout</button>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</template>

<style>

</style>