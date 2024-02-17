import type { UserAuth } from "@/models/userAuth";
import { UserEntity } from "@/models/userEntity";
import { type Commit, createStore } from "vuex/types/index.js"

export default createStore({
    state: {
        user: new UserEntity(),
        token: ''
    },
    mutations: {
        SET_AUTH: (state: {user: UserEntity, token: string}, auth: UserAuth) => {
            state.user = auth.user;
            state.token = auth.token;
        }
    },
    actions: {
        setAuth: ({commit}: {commit: Commit}, auth: UserAuth) => commit('SET_AUTh', auth)
    },
    modules: {

    }
});