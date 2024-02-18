import { config } from "@/config"
import type { LoginDto } from "@/models/loginDto";
import axios from "axios";
import type { MyResponse } from "../models/myResponse";
import type { UserAuth } from "@/models/userAuth";
import type { RegisterDto } from "@/models/registerDto";


const API_URL = config.apiUrl;

class AuthService {
    login(loginDto: LoginDto): Promise<UserAuth> {
      return axios
        .post(API_URL + '/login', {
          loginDto
        })
        .then(response => {
            const res = response.data as MyResponse<UserAuth>;
          if (res.responseResult == 'Success' && res.value) {
            localStorage.setItem('user', JSON.stringify(res.value.user));
            localStorage.setItem('token', JSON.stringify(res.value.token));
          }
  
          return response.data.value;
        });
    }
  
    logout() {
      localStorage.removeItem('user');
      localStorage.removeItem('token');
    }
  
    register(register: RegisterDto) {
      return axios.post(API_URL + '/register', {
        register
      });
    }
  }
  
  export default new AuthService();