import config from "@/config"
import type { LoginDto } from "../models/loginDto";
import type { RegisterDto } from "../models/registerDto";
import { MyResponse } from "./myResponse";
import { router, fetchWrapper } from "../helpers";
import { UserAuth } from "../models/userAuth";

export const userService = {
    login,
    logout
};

function login(loginDto: LoginDto) {
    return fetchWrapper.post(`${config.apiUrl}/login`, loginDto)
        ?.then((data) => {
            const response = data as MyResponse<UserAuth>;

            if (response.responseResult == 'Success' && response.value) {
                localStorage.setItem('user', JSON.stringify(response.value.user));
                localStorage.setItem('token', JSON.stringify(response.value.token));
                router.push('/');
            }
            else {
                router.push('/login');
            }
        });
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
    localStorage.removeItem('token');
}