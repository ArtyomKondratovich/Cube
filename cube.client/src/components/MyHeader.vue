<script setup lang="ts">
    import { useAuthStore } from '../store/auth.store';
    
    const logout = () : void => {
        const store = useAuthStore();
        store.logout();
    }
</script>

<template>
        <div class="nav-blocks">
            <div class="logo">
                <router-link to="/home/posts">
                    <img src="../assets/icons/cubeIcon.png">
                </router-link>
            </div>
            <div class="links">
                <ul>
                    <li id="home">
                        <router-link to="/home/posts">Profile</router-link>
                    </li>
                    <li id="signin">
                        <router-link to="/login">SignIn</router-link>
                    </li>
                    <li id="signup">
                        <router-link to="/register">SignUp</router-link>
                    </li>
                    <li id="logout">
                        <a @click="logout">Logout</a>
                    </li>
                </ul>
            </div>
        </div>
</template>

<style>
    .nav-blocks {
        display: flex;
        justify-content: space-between;
        width: 62%;
    }

    .links ul{
        display: flex;
        padding: 0;

    }
    .links li{
        list-style-type: none;
        margin-left: 5px;
        margin-right: 5px;
    }
    a{
        text-decoration: none;
        cursor: pointer;
    }
    a, a:visited, a:hover, a:active {
        color: white;
    }
</style>