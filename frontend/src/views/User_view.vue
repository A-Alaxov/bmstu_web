<template>
  <div>
    <header>
      <header-bar></header-bar>
    </header>
    <div class="page-with__image">
      <sideBar active-index="userView"></sideBar>
      <div class="login">
        <el-card>
          <h2>@{{ getUser.login }}</h2>
          <el-form
              class="login-form"
              :model="model"
              :rules="rules"
              ref="form"
              @submit.native.prevent="patchReq"
          >
            <el-form-item prop="name">
              <el-input
                  v-model="model.name"
                  :placeholder="getUser.name_"
              >
              </el-input>
            </el-form-item>
            <el-form-item prop="surname">
              <el-input
                  v-model="model.surname"
                  :placeholder="getUser.surname"
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
              >Изменить
              </el-button>
            </el-form-item>
            <el-button
                :loading="loading"
                class="login-button"
                type="danger"
                @click.native="handleLogOut"
            >Выйти
            </el-button>
          </el-form>
        </el-card>
      </div>
      <img src="@/assets/comp.png" alt="">
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import {mapGetters, mapMutations} from "vuex";
import { editUserInfo } from "@/utils/requests/user";
import sideBar from '@/components/Side_bar.vue';
import headerBar from '@/components/Header_bar.vue';
import {logoutUser} from "@/utils/requests/auth";
import router from "@/router";

export default Vue.extend({
  name: "userView",
  computed: {
    ...mapGetters([
      "getUser"
    ])
  },
  data() {
    return {
      model: {
        name: "",
        surname: ""
      },
      loading: false,
      rules: {
        name: [
          {
            required: true,
            message: "Имя - обязательно",
            trigger: "blur"
          },
          {
            min: 3,
            message: "Имя должно быть не короче 3 символов",
            trigger: "blur"
          }
        ],
        surname: [
          {
            required: true,
            message: "Фамилия - обязательно",
            trigger: "blur"
          },
          {
            min: 3,
            message: "Фамилия должна быть не короче 3 символов",
            trigger: "blur"
          }
        ],
      }
    };
  },
  components: {
    sideBar,
    headerBar,
  },
  methods: {
    ...mapMutations([
      "setUser",
      "setCurWorkplace"
    ]),
    handleLogOut() {
      logoutUser()
          .then(response => {
            if (!response.ok)
              throw new Error("Something went wrong")

            this.setUser(null);
            this.setCurWorkplace(null);
            router.push("/login")
          })
          .catch((error) => {
            console.log("logout:", error);
          })
    },
    async patchReq() {
      this.loading = true;

      await editUserInfo(
          this.model.name,
          this.model.surname,
      )
          .then(response => {
            if (!response.ok)
              throw new Error("Что-то пошло не так");

            return response.json();
          })
          .then(data => {
            this.setUser(data);
          })
          .catch((error) => {
            console.log("put:", error);
          })
          .finally(() => {
            this.loading = false;
          })
    },
  }
});
</script>

<style scoped>

h2 {
  font-size: 16px;
  font-weight: bold;
}

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
