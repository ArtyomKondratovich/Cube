import { userService } from "@/services";
import { router } from "@/helpers";

const user = JSON.parse(localStorage.getItem('user'));
const initialState = user
    ? { status: { loggedIn: true }, user }
    : { status: {}, user: null };

export const authentication = {
    namespaced: true,
    state: initialState,
    actions: {
        login({ dispatch, commit }, { email, password }) {
            commit('loginRequest', { email });

            userService.login(email, password)
                .then(
                    user => {
                        commit('loginSuccess', user);
                        router.push('/');
                    },
                    error => {
                        commit('loginFailure', error);
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        logout({ commit }) {
            userService.logout();
            commit('logout');
        },
        register({ commit }, { name, surname, dateOfBirth, email, password }) {

            userService.register(name, surname, dateOfBirth, email, password)
                .then(
                    register =>
                    {
                        commit('registerSuccess', register);
                        router.push('/login');
                    },
                    error =>
                    {
                        commit('registerFailrue', error);
                        router.push('/register');
                    }
                );
        }
    },
    mutations: {
        loginRequest(state, user) {
            state.status = { loggingIn: true };
            state.user = user;
        },
        loginSuccess(state, user) {
            state.status = { loggedIn: true };
            state.user = user;
        },
        loginFailure(state) {
            state.status = {};
            state.user = null;
        },
        logout(state) {
            state.status = {};
            state.user = null;
        },
        registerSuccess(state)
        {
            state.status = {};
            state.user = null;
        },
        registerFailrue(state)
        {
            state.status = {};
            state.user = null;
        }
    }
}