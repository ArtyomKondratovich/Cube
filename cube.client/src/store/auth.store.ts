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
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css'

interface AuthState {
  user: IUser | null;
  token: string;
}

export const useAuthStore = defineStore(
  'auth', {
    state: (): AuthState => ({
      user: {} as IUser,
      token: ''
    }),
    actions: {
      async login(loginInput: ILoginInput){
        return axios.post(`${config.apiUrl}/User/login`, loginInput, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
      },
      async register(registerInput: IRegisterInput){
          axios.post(`${config.apiUrl}/User/register`, registerInput, {
              headers: {
                  'Content-Type': 'application/json'
              }
          }).then(async (response) => {
              const data = response.data as IResponse<IAuth>;

              if (data.responseResult == 'Success')
              {
                toast.success('You register successfully!');
                await new Promise(resolve => setTimeout(resolve, 2000));
                router.push('/login');
              }
              else
              {
                toast.error(data.responseResult);
                await new Promise(resolve => setTimeout(resolve, 2000));
                router.push('/register');
              }
          }).catch(async error => {
              //handling error
              toast.error(error);
              await new Promise(resolve => setTimeout(resolve, 2000));
              router.push('/register');
          });
      },
      logout(){
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        this.$state.token = '';
        this.$state.user = {} as IUser;
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