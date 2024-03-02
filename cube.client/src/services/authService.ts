import type { ILoginInput, IRegisterInput, IResponse } from "@/api/types";
import { config } from "@/config"
import axios from 'axios'

const API_URL = config.apiUrl;

class AuthService {
    async login(loginInput: ILoginInput): Promise<IResponse<any>> {
      return axios
        .post(API_URL + '/login', loginInput)
        .then(response => {
            const res = response.data as IResponse<any>;
            if (res.responseResult == 'Success' && res.value) {
              localStorage.setItem('user', JSON.stringify(res.value.user));
              localStorage.setItem('token', JSON.stringify(res.value.token));
            }
  
          return response.data;
        });
    }
  
    logout() {
      localStorage.removeItem('user');
      localStorage.removeItem('token');
    }
  
    register(registerInput: IRegisterInput) {
      return axios.post(API_URL + '/register', registerInput);
    }
  }
  
  export default new AuthService();