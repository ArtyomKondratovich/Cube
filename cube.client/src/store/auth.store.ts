import { defineStore, type Store } from 'pinia'
import { 
  type IUser,
  type IResponse
} from '@/api/types';
import config from '@/config';
import axios from 'axios';
import router from '@/helpers/router';
import 'vue3-toastify/dist/index.css'

interface IAuthState {
  user: IUser | null;
  token: string;
}

export interface IAuthStore extends Store {
  logout(): void;
  validateToken(): boolean;
  isLoggedIn: boolean;
  $state: {
    user: IUser;
    token: string;
  };
}

export const useAuthStore = defineStore(
  'auth', {
    state: (): IAuthState => ({
      user: (JSON.parse(localStorage.getItem('user') ?? '{}')) as IUser,
      token: localStorage.getItem('token') ?? ''
    }),
    actions: {
      logout(): void{
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        this.$state.token = '';
        this.$state.user = {} as IUser;
        router.push('/login');
      },
      validateToken(): boolean {
        const token = localStorage.getItem('token');

        if (!token) {
          return false;
        }

        axios.post(`${config.apiUrl}/User/validateToken`, { token }, {
          headers: {
            'Content-Type': 'application/json'
          }
        }).then((response) => {
          const data = response.data as IResponse<string>;

          if (data.responseResult == 'Success' && data.value) {
            return true;
          }
        })
        .catch(error => {
          console.log(error);
        })

        return false;
      }
    },
    getters: {
      isLoggedIn(): boolean {
        return this.token != '';
      }
    }
  }
);