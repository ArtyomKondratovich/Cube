<template>
  <div class="log-form">
  <h2>Login to your account</h2>
  <form>
    <input type="email" title="email" v-model="email" placeholder="example@gmail.com" />
    <input type="password" title="username" v-model="password" placeholder="password" />
    <button type="submit" v-on:click.prevent = "Onsubmit" class="btn">Login</button>
  </form>
</div>
</template>

<script lang="ts"> 
import { useAuthStore } from '../store/auth.store';
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'Login',
  components: {},
  data() {
    return {
      email: '',
      password: '',
    }
  },
  methods: {
    async Onsubmit(){
      const store = useAuthStore();
      let email = this.email;
      let password = this.password;
      await store.login({email, password});
    }
  },
  computed: {
    
  }
});
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