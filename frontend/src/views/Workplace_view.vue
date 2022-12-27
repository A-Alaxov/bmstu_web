<template>
  <div>
    <header>
      <header-bar></header-bar>
    </header>
    <div class="page-with__image">
      <sideBar active-index="workplaceView"></sideBar>
      <div class="login">
        <el-card>
          <h2>@{{ getUser.login }}</h2>
          <el-form
              class="login-form"
              ref="form"
          >
            <el-form-item>
              Role: {{ getCurWorkplace.permission_ }}
            </el-form-item>
            <el-form-item>
              Company: {{ getCurWorkplace.company.title }}
            </el-form-item>
            <el-form-item>
              Department: {{ getCurWorkplace.department === null ? null : getCurWorkplace.department.title }}
            </el-form-item>
            <el-form-item>
              <el-button
                  v-if="isFounder"
                  :loading="loading"
                  class="login-button"
                  type="primary"
                  @click.native="giveMeEditModal"
                  block
              >Изменить компанию
              </el-button>
            </el-form-item>
            <el-button
                v-if="isFounder"
                :loading="loading"
                class="login-button"
                type="danger"
                @click.native="deleteCompany"
            >Удалить компанию
            </el-button>
          </el-form>
        </el-card>
      </div>
      <img src="@/assets/comp.png" alt="">
    </div>
    <Modal_window v-if="showEditModal">
      <template v-slot:header>
        <h3 class="small-h3">Изменение компании</h3>
      </template>
      <template v-slot:body>
        <el-form
            class="login-form"
            :model="model"
            :rules="rules"
            ref="form"
        >
          <el-form-item prop="inputTitle">
            <el-input
                v-model="model.inputTitle"
                placeholder="Название компании"/>
          </el-form-item>
          <el-form-item prop="inputFoundationYear">
            <el-input
                v-model="model.inputFoundationYear"
                placeholder="Год основания"/>
          </el-form-item>
        </el-form>
      </template>
      <template v-slot:footer>
        <div class="input-string">
          <el-button @click="closeEditModel" type="primary">Отмена</el-button>
          <el-button @click="validatedEditFormConfirm" type="primary" :loading="loading">Изменить</el-button>
        </div>
      </template>
    </Modal_window>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import {mapGetters, mapMutations} from "vuex";
import sideBar from '@/components/Side_bar.vue';
import headerBar from '@/components/Header_bar.vue';
import { putCompany, removeCompany } from "@/utils/requests/company";
import Modal_window from '@/components/Modal.vue'
import router from "@/router";

export default Vue.extend({
  name: "workplaceView",
  computed: {
    ...mapGetters([
      "getUser",
      "getCurWorkplace",
      "isFounder"
    ])
  },
  data() {
    return {
      model: {
        inputTitle: "",
        inputFoundationYear: 0,
      },
      showEditModal: false,
      loading: false,
      rules: {
        inputTitle: [
          {
            required: true,
            message: "Имя - обязательно",
            trigger: "blur"
          },
          {
            min: 3,
            message: "Название должно быть не короче 3 символов",
            trigger: "blur"
          }
        ],
        inputFoundationYear: [
          {
            required: true,
            message: "Год основания - обязательно",
            trigger: "blur"
          },
        ],
      }
    }
  },
  components: {
    sideBar,
    headerBar,
    Modal_window,
  },
  methods: {
    ...mapMutations([
      "setCurWorkplace"
    ]),
    closeEditModel() {
      this.showEditModal = false;
    },
    giveMeEditModal() {
      this.showEditModal = true;
    },
    async validatedEditFormConfirm() {
      await this.putReq();
    },
    async putReq() {
      this.loading = true;

      await putCompany(
          this.model.inputTitle,
          this.model.inputFoundationYear,
      )
          .then(response => {
            if (!response.ok) {
              return;
            }
            return response.json();
          })
          .then(item => {
            this.setCurWorkplace(item);
            this.closeEditModel();
          })
          .catch((error) => {
            console.log("put:", error);
            this.closeEditModel();
          })
          .finally(() => {
            this.loading = false;
          })
    },
    async deleteCompany() {
      await removeCompany()
          .then(response => {
            if (!response.ok) {
              return;
            }
            this.setCurWorkplace(null);
            router.push('/workplaceSelection');
          })
          .catch((error) => {
            console.log("delete:", error);
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
