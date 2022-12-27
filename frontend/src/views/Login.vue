<template>
  <div class="page-with__image">
    <div class="login">
      <el-card>
        <h2>Вход</h2>
        <el-form
            class="login-form"
            :model="model"
            :rules="rules"
            ref="form"
            @submit.native.prevent="login"
        >
          <el-form-item prop="username">
            <el-input
                v-model="model.username"
                placeholder="Имя пользователя"
                prefixIcon="el-icon-user"
            >
            </el-input>
          </el-form-item>
          <el-form-item prop="password">
            <el-input
                v-model="model.password"
                placeholder="Пароль"
                show-password
                prefixIcon="el-icon-lock"
            >
            </el-input>
          </el-form-item>
          <el-form-item>
            <el-button
                :loading="loading"
                class="login-button"
                type="primary"
                native-type="submit"
                block
            >Войти
            </el-button>
          </el-form-item>
          <router-link class="forgot-password" to="/registration">Зарегистрироваться</router-link>
        </el-form>
      </el-card>
    </div>
    <img src="@/assets/comp.png" alt="">
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapMutations } from "vuex";
import { loginUser } from "@/utils/requests/auth";
import router from "../router";
import { getUserInfo } from "@/utils/requests/user";

export default Vue.extend({
  name: "Login_page",
  data() {
    return {
      model: {
        username: "",
        password: ""
      },
      loading: false,
      rules: {
        username: [
          {
            required: true,
            message: "Имя пользователя - обязательно",
            trigger: "blur"
          },
          {
            min: 5,
            message: "Имя пользователся должно быть не менее 5 символов",
            trigger: "blur"
          }
        ],
        password: [
          {
            required: true,
            message: "Пароль - обязателен",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
    ...mapMutations([
      "setUser",
    ]),
    async getUser() {
      await getUserInfo()
          .then(response => {
            if (!response.ok)
              throw new Error("Что-то пошло не так");

            return response.json();
          })
          .then(data => {
            this.setUser(data)
          })
          .catch(error => {
            console.log("get userInfo: ", error)
          })
    },
    async login() {
      this.loading = true;

      await loginUser(this.model.username, this.model.password)
          .then(response => {
            if (response.status === 403) {
              this.$notify({
                group: 'foo',
                type: 'error',
                text: 'Неверное имя пользователя или пароль'
              });
              throw new Error('promise chain cancelled');
            }
            if (!response.ok) {
              this.$notify({
                group: 'foo',
                type: 'error',
                text: 'Что-то пошло не так'
              });
              throw new Error('promise chain cancelled');
            }
          })
          .then(() => {
            this.$notify({
              group: 'foo',
              type: 'success',
              text: 'Авторизация успешна'
            });

            this.getUser()
                .then(() => {
                  if (this.$route.query.returnUrl)
                    router.push(this.$route.query.returnUrl.toString())
                  else
                    router.push('/workplaceSelection')
                })
          })
          .catch(error => {
            console.log("login: ", error)
          })
          .finally(() => {
            this.loading = false;
          })
    }
  }
});
</script>

<style scoped>
.login {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
}

.login-button {
  width: 100%;
  margin-top: 40px;
}

.login-form {
  width: 290px;
}

.forgot-password {
  margin-top: 10px;
}
</style>
<style lang="scss">
$teal: rgb(0, 124, 137);
.el-button--primary {
  background: $teal;
  border-color: $teal;

  &:hover,
  &.active,
  &:focus {
    background: lighten($teal, 7);
    border-color: lighten($teal, 7);
  }
}

.login .el-input__inner:hover {
  border-color: $teal;
}

.login .el-input__prefix {
  background: rgb(238, 237, 234);
  height: calc(100% - 2px);
  left: 1px;
  top: 1px;
  border-radius: 3px;

  .el-input__icon {
    width: 30px;
  }
}

.login .el-input input {
  padding-left: 35px;
}

.login .el-card {
  padding-top: 0;
  padding-bottom: 30px;
}

h2 {
  letter-spacing: 1px;
  font-family: Roboto, sans-serif;
  padding-bottom: 20px;
}

a {
  color: $teal;
  text-decoration: none;

  &:hover,
  &:active,
  &:focus {
    color: lighten($teal, 7);
  }
}

.login .el-card {
  width: 340px;
  display: flex;
  justify-content: center;
}
</style>
