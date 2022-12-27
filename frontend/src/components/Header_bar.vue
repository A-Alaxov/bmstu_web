<template>
  <div class="header-text">
    <img src="@/assets/beer.svg" alt="Лого">
    My Jira
    <el-dropdown style="height: 45px">
      <span class="el-dropdown-link">
        <img class="avatar" src="@/assets/user.svg" alt="Пользователь">
      </span>
      <el-dropdown-menu slot="dropdown">
        <el-dropdown-item style="color: black" disabled>{{ this.getUser.username }}</el-dropdown-item>
        <el-dropdown-item style="color: black" disabled>{{ this.getUser.name_ + " " + this.getUser.surname }}</el-dropdown-item>
        <el-dropdown-item style="color: red" divided @click.native="handleLogOut">Выйти</el-dropdown-item>
      </el-dropdown-menu>
    </el-dropdown>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { logoutUser } from "@/utils/requests/auth";
import router from "@/router";
import { mapGetters, mapMutations } from "vuex";

export default Vue.extend({
  name: "headerBar",
  computed: {
    ...mapGetters([
      "getUser"
    ])
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
  }
});
</script>

<style scoped>
@import '../assets/css/my_styles.css';
</style>
