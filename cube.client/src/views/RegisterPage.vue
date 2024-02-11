<template>
    <div>
        <h2>Login</h2>
        <form @submit.prevent="handleSubmit">
            <div class="form-group">
                <label for="name">name</label>
                <input type="text" v-model="name" name="name" class="form-control" :class="{ 'is-invalid': submitted && !name }" />
                <div v-show="submitted && !name" class="invalid-feedback">name is required</div>
            </div>
            <div class="form-group">
                <label for="surname">surname</label>
                <input type="text" v-model="surname" name="surname" class="form-control" :class="{ 'is-invalid': submitted && !surname }" />
                <div v-show="submitted && !surname" class="invalid-feedback">surname is required</div>
            </div>
            <div class="form-group">
                <label for="dateOfBirth">dateOfBirth</label>
                <input type="date" v-model="dateOfBirth" name="dateOfBirth" class="form-control"/>
            </div>
            <div class="form-group">
                <label for="email">email</label>
                <input type="email" v-model="email" name="email" class="form-control" :class="{ 'is-invalid': submitted && !email }" />
                <div v-show="submitted && !email" class="invalid-feedback">email is required</div>
            </div>
            <div class="form-group">
                <label htmlFor="password">Password</label>
                <input type="password" v-model="password" name="password" class="form-control" :class="{ 'is-invalid': submitted && !password }" />
                <div v-show="submitted && !password" class="invalid-feedback">Password is required</div>
            </div>
            <div class="form-group">
                <button class="btn btn-primary" :disabled="loggingIn">Login</button>
            </div>
        </form>
    </div>
</template>

<script>
    export default {
        name: "Register",
        data() {
            return {
                name: '',
                surname: '',
                dateOfBirth: null,
                email: '',
                password: '',
                submitted: false
            }
        },
        computed: {
            loggingIn() {
                return this.$store.state.authentication.status.loggingIn;
            }
        },
        created() {
            // reset login status
            this.$store.dispatch('authentication/logout');
        },
        methods: {
            handleSubmit(e) {
                this.submitted = true;
                const {
                    name,
                    surname,
                    dateOfBirth,
                    email,
                    password } = this;
                const { dispatch } = this.$store;
                if (email && password) {
                    dispatch('authentication/register', { name, surname, dateOfBirth, email, password });
                }
            }
        }
    };
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