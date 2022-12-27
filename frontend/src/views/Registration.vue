<template>
  <div class="page-with__image">
    <div class="login">
      <el-card>
        <h2>Registration</h2>
        <el-form
            class="login-form"
            :model="model"
            :rules="rules"
            ref="form"
            @submit.native.prevent="registration"
        >
          <el-form-item prop="username">
            <el-input
                v-model="model.username"
                placeholder="Имя пользователя"
                prefixIcon="el-icon-user"
            >
            </el-input>
          </el-form-item>
          <el-form-item prop="first_name">
            <el-input
                v-model="model.first_name"
                placeholder="Имя"
                type="name"
            ></el-input>
          </el-form-item>
          <el-form-item prop="last_name">
            <el-input
                v-model="model.last_name"
                placeholder="Фамилия"
                type="name"
            ></el-input>
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
          <el-form-item prop="password_confirmation">
            <el-input
                v-model="model.password_confirmation"
                placeholder="Подтвердите пароль"
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
            >Зарегистрироваться
            </el-button>
          </el-form-item>
          Уже есть аккаунт?
          <router-link to="/login">Войдите</router-link>
        </el-form>
      </el-card>
    </div>
    <img src="@/assets/comp.png" alt="">
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { registerUser } from "@/utils/requests/auth";
import router from "../router";

export default Vue.extend({
  name: "Registration_page",
  data() {
    const passCheck = (rule: any, value: string, callback: any) => {
      if (value === '') {
        callback(new Error('Пароль пуст'));
      } else {
        if (this.$data.model.password_strength !== 4) {
          callback("Too weak");
          return;
        }
        callback();
      }
    }
    const passCheck2 = (rule: any, value: string, callback: any) => {
      if (value === '') {
        callback(new Error('Пароль пуст'));
      } else if (value !== this.$data.model.password) {
        callback(new Error('Пароли не совпадают'))
      } else {
        callback();
      }
    }
    return {
      model: {
        username: "",
        first_name: "",
        last_name: "",
        email: "",
        password: "",
        password_confirmation: ""
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
            min: 3,
            message: "Имя пользователя должно быть не короче 3 символов",
            trigger: "blur"
          }
        ],
        first_name: [
          {
            required: true,
            message: "Имя - обязательно",
            trigger: "blur"
          },
        ],
        last_name: [],
        email: [
          {
            required: true,
            message: "Адрес почты - обязателен",
            trigger: "blur"
          },
          {
            type: "email",
            message: "Неверный адрес почты",
            trigger: "blur"
          }
        ],
        password: [
          {
            required: true,
            message: "Пароль - обязательное поле",
            trigger: "blur"
          },
          {
            validator: passCheck,
            trigger: "change"
          }
        ],
        password_confirmation: [
          {
            required: true,
            message: "Пожалуйста, подтвердите пароль",
            trigger: "blur"
          },
          {
            validator: passCheck2,
            trigger: "change"
          }
        ]
      }
    };
  },
  methods: {
    async registration() {
      this.loading = true;

      await registerUser(
          this.model.username,
          this.model.password,
          this.model.first_name,
          this.model.last_name
      )
          .then(response => {
            if (!response.ok) {
              this.$notify({
                group: 'foo',
                type: 'error',
                text: 'Что-то пошло не так'
              });
              throw new Error('promise chain cancelled');
            }
            this.$notify({
              group: 'foo',
              type: 'success',
              text: 'Регистрация успешна'
            });
            router.push('/login')
          })
          .catch(error => {
            console.log("registration: ", error)
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
