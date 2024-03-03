<script setup lang="ts">
    import { useAuthStore } from '../store/auth.store';
    
    const logout = () : void => {
        const store = useAuthStore();
        store.logout();
    }
</script>

<template>
    <nav>
        <div class="nav-blocks">
            <div class="logo">
                <router-link to = "/">Cube</router-link>
            </div>
            <div class="links">
                <ul>
                    <li id="home">
                        <router-link to="/" class="nav-link active" aria-current="page">Profile</router-link>
                    </li>
                    <li id="signin">
                        <router-link to="/login" class="nav-link">SignIn</router-link>
                    </li>
                    <li id="signup">
                        <router-link to="/register" class="nav-link disabled" aria-disabled="true">SignUp</router-link>
                    </li>
                    <li id="logout">
                        <button @click="logout">Logout</button>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</template>

<style>
    nav{
        width: 100%;
        height: 15%;
    }
    .logo{
        width:10%;
    }
    .nav-blocks div {
        display: inline-block;
    }
    .links{
        width: 20%;
        margin-left: 70%;
        display: block;
    }
    .links li{
        list-style-type: none;
        display: inline;
        margin-left: 3%;
        margin-right: 3%;
    }
    a{
        text-decoration: none;

    }
    a, a:visited, a:hover, a:active {
        color: grey;
    }
</style>