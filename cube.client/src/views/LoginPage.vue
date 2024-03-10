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
            <button type="submit" v-on:click.prevent = "Onsubmit" v-bind:disabled="submitted" class="btn">Login</button>
          </li>
        </ul>
      </form>
    </div>
  </div>
</template>

<script lang="ts"> 
import router from '@/helpers/router';
import { useAuthStore } from '../store/auth.store';
import { defineComponent } from 'vue';
import { toast } from 'vue3-toastify';

export default defineComponent({
  name: 'Login',
  components: {},
  data() {
    return {
      submitted: false,
      email: '',
      password: '',
    }
  },
  methods: {
    async Onsubmit(){
      this.submitted = true;
      const store = useAuthStore();
      let email = this.email;
      let password = this.password;
      store.login({email, password})
        .then(async (response) => {
          const data = response.data;
      
          if (data.responseResult == 'Success' &&  data.value)
          {
            toast.success('Authentication was successful');
            await new Promise(resolve => setTimeout(resolve, 2000));
            localStorage.setItem('token', data.value.token);
            localStorage.setItem('user', JSON.stringify(data.value.user));
            this.$router.push('/home');
          }
          else{
            toast.error(data.responseResult);
              this.$router.push('/login');
          }
        })
        .catch(error => {
            //handling error
            toast.error(error);
            this.$router.push('/login');
        });
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