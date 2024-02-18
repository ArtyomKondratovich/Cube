import { createStore } from "vuex";

import Auth from "./modules/auth.module";

export default createStore({
  modules: {
    Auth
  }
});