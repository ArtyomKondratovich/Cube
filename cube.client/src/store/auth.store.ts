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
      user: JSON.parse(localStorage.getItem('user') ?? '{}') as IUser,
      token: localStorage.getItem('token') ?? ''
    }),
    actions: {
      async login(loginInput: ILoginInput){
        axios.post(`${config.apiUrl}/User/login`, loginInput, {
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(async (response) => {
            const data = response.data;

            if (data.responseResult == 'Success' &&  data.value)
            {
              toast.success('Authentication was successful');
              await new Promise(resolve => setTimeout(resolve, 2000));
              localStorage.setItem('token', data.value.token);
              localStorage.setItem('user', JSON.stringify(data.value.user));
              router.push('/home');
            }
            else{
              toast.error(data.responseResult);
              router.push('/login');
            }
        })
        .catch(error => {
            //handling error
            toast.error(error);
            router.push('/login');
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