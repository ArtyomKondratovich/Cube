<template>
    <div>
        <h2>Login</h2>
        <form @submit.prevent="handleRegister">
            <div class="form-group">
                <label for="name">name</label>
                <input type="text" v-model="user.name" name="name" class="form-control" :class="{ 'is-invalid': submitted && !user.name }" />
                <div v-show="submitted && !user.name" class="invalid-feedback">name is required</div>
            </div>
            <div class="form-group">
                <label for="surname">surname</label>
                <input type="text" v-model="user.surname" name="surname" class="form-control" :class="{ 'is-invalid': submitted && !user.surname }" />
                <div v-show="submitted && !user.surname" class="invalid-feedback">surname is required</div>
            </div>
            <div class="form-group">
                <label for="dateOfBirth">dateOfBirth</label>
                <input type="date" v-model="user.dateOfBirth" name="dateOfBirth" class="form-control"/>
            </div>
            <div class="form-group">
                <label for="email">email</label>
                <input type="email" v-model="user.email" name="email" class="form-control" :class="{ 'is-invalid': submitted && !user.email }" />
                <div v-show="submitted && !user.email" class="invalid-feedback">email is required</div>
            </div>
            <div class="form-group">
                <label htmlFor="password">Password</label>
                <input type="password" v-model="user.password" name="password" class="form-control" :class="{ 'is-invalid': submitted && !user.password }" />
                <div v-show="submitted && !user.password" class="invalid-feedback">Password is required</div>
            </div>
            <div class="form-group">
                <button class="btn btn-primary" :disabled="isLoggedIn">Login</button>
            </div>
        </form>
    </div>
</template>

<script lang="ts">
import router from "@/helpers/router";
import { RegisterDto } from "@/models/registerDto";
import { Component, Vue } from "vue-property-decorator";
import { namespace } from "vuex-class";

const Auth = namespace("Auth");

@Component
export default class Register extends Vue {
  public user: RegisterDto = { name: "", surname: "", dateOfBirth: null, email: "", password: "" };

  public submitted: boolean = false;
  private successful: boolean = false;
  private message: string = "";

  @Auth.Getter
  public isLoggedIn!: boolean;

  @Auth.Action
  private register!: (userDto: RegisterDto) => Promise<any>;

  mounted() {
    if (this.isLoggedIn) {
        router.push("/");
    }
  }

  handleRegister() {
    this.message = "";
    this.submitted = true;

    this.register(this.user).then(
          (data) => {
            this.message = data.message;
            this.successful = true;
          },
          (error) => {
            this.message = error;
            this.successful = false;
          }
        );
  }
}
</script>

<style scoped>
    .form-signin {
        max-width: 330px;
        padding: 1rem;
    }

        .form-signin .form-floating:focus-within {
            z-index: 2;
        }

        .form-signin input[type="email"] {
            margin-bottom: -1px;
            border-bottom-right-radius: 0;
            border-bottom-left-radius: 0;
        }

        .form-signin input[type="password"] {
            margin-bottom: 10px;
            border-top-left-radius: 0;
            border-top-right-radius: 0;
        }
</style>