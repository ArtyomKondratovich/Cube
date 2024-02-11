import config from "@/config"
import { authHeader, router } from "@/helpers";

export const userService = {
    login,
    register,
    logout,
    getAll
};

function login(email, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    };

    return fetch(`${config.apiUrl}/login`, requestOptions)
        .then(handleResponse)
        .then(response => {
            // login successful if there's a jwt token in the response
            if (response.value.token) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(response));
            }

            return response;
        });
}

function register(name, surname, dateOfBirth, email, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, surname, dateOfBirth, email, password })
    };

    return fetch(`${config.apiUrl}/register`, requestOptions)
        .then(handleResponse)
        .then(response => {
            // register successful if there's a true value in the response
            if (response.value) {
                localStorage.setItem('register', JSON.stringify(response))
            }

            return response;
        });
}
function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
}

function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/users`, requestOptions).then(handleResponse);
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout();
                location.reload(true);
            }

            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}