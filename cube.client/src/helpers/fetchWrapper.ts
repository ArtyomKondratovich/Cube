import { useAuthStore } from "@/stores"
import config from "@/config"

export const fetchWrapper = {
    get: request('GET'),
    post: request('POST'),
    put: request('PUT'),
    delete: request('DELETE')
};

function request(method: string) {
    return (url: string, body?: any) => {
        const requestOptions = {
            method,
            headers: generateHeader(url),
            body: body ?? ''
        };

        if (body && body != '') {
            requestOptions.body = JSON.stringify(body);
            return fetch(url, requestOptions)
                .then((response) => response.json());
        }

        return null;
    }
}

function generateHeader(url: string): HeadersInit {
    // return auth header with jwt if user is logged in and request is to the api url
    const store = useAuthStore();
    const isApiUrl = url.startsWith(`${config.apiUrl}`);

    if (store.isAuthenticate && isApiUrl) {
        return {
            'Content-Type': 'application/json',
            'Authorization': store.authHeader
        };
    } else {
        return {
            'Content-Type': 'application/json'
        };
    }
}