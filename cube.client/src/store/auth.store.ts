import { defineStore } from 'pinia'
import { 
  type IUser,
  type IAuth,
  type ILoginInput,
  type IResponse,
  type IRegisterInput
} from '@/api/types';
import config from '@/config';
import axios from 'axios';
import router from '@/helpers/router';

interface AuthState {
  user: IUser | null;
  token: string;
}

export const useAuthStore = defineStore(
  'auth', {
    state: (): AuthState => ({
      user: null,
      token: ''
    }),
    actions: {
      async login(loginInput: ILoginInput){
        axios.post(`${config.apiUrl}/login`, loginInput, {
            headers: {
                'Content-Type': 'application/json'
            }
        }).then((response) => {
            const data = response.data;

            if (data.responseResult == 'Success' &&  data.value)
            {
                localStorage.setItem('token', data.value.token);
                localStorage.setItem('user', JSON.stringify(data.value.user));
                this.user = data.value.user;
                this.token = data.value.token;
                router.push('/');
            }
        })
        .catch(error => {
            //handling error
            router.push('/login');
        });
      },
      async register(registerInput: IRegisterInput){
          axios.post(`${config.apiUrl}/register`, registerInput, {
              headers: {
                  'Content-Type': 'application/json'
              }
          }).then((response) => {
              const data = response.data.json() as IResponse<boolean>;

              if (data.responseResult == 'Success')
              {
                  router.push('/login');
              }
          }).catch(error => {
              //handling error
              router.push('/register')
          });
      },
      logout(){
        this.user = null;
        this.token = '';
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        router.push('/login');
      }
    },
    getters: {
      isLoggedIn(): boolean {
        return this.token != '';
      } 
    }
  }
);