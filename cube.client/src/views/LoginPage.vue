<template>
  <div class="login-form">
    <div class="log-block">
      <h2>SignIn</h2>
      <form>
        <ul>
          <li>
            <input type="email" title="email" v-model="email" placeholder="example@gmail.com" />
          </li>
          <li>
            <input type="password" title="username" v-model="password" placeholder="password" />
          </li>
          <li>
            <button type="submit" v-on:click.prevent = "Onsubmit" class="btn">Login</button>
          </li>
        </ul>
      </form>
    </div>
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
    .login-form {
      width: 20%;
      margin-top: 10%;
      margin-left: 40%;
      margin-right: 40%;
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