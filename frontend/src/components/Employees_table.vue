<template>
  <div>
    <section class="search">
      <div class="qs-container search__content-wrapper">
        <el-input type="search" placeholder="Поиск..." v-model="search">
          <i slot="prefix" class="el-input__icon el-icon-search"></i>
        </el-input>
        <el-button v-if="showOptions && isHR"
            @click="giveMeAddModal"
            class="add-button"
            type="primary">
          Новый сотрудник
        </el-button>
      </div>
    </section>
    <section class="goods">
      <async_employees :request="requestItems">
        <template v-slot:default="{ pending, error }">
          <div v-if="pending" class="text-center">
            <el-table
                v-loading="true"
                :default-sort="{prop: 'name', order: 'ascending'}"
                sortable="custom"
                style="width: 100%; margin-bottom: 20px;"
                row-key="id">
              <el-table-column
                  type="selection"
                  width="45">
              </el-table-column>
              <el-table-column
                  prop="name"
                  label="Название проекта"
                  sortable
                  sort-by="name">
                <template slot-scope="scope">
                  {{ scope.row.name }}
                </template>
              </el-table-column>
              <el-table-column
                  prop="startDate"
                  width="150"
                  label="Дата начала"
                  align="right"
                  sortable
                  sort-by="startDate">
                <template slot-scope="scope">
                  {{ timeFormat(scope.row.startDate, "DD-MM-YYYY") }}
                </template>
              </el-table-column>
              <el-table-column
                  prop="endDate"
                  width="150"
                  label="Дата окончания"
                  align="right"
                  sortable
                  sort-by="endDate">
                <template slot-scope="scope">
                  {{ timeFormat(scope.row.endDate, "DD-MM-YYYY") }}
                </template>
              </el-table-column>
              <el-table-column
                  align="right"
                  width="100">
                <template slot-scope="scope">
                  <el-button
                      size="mini"
                      type="danger"
                      @click="deleteReq(scope.row)">Удалить</el-button>
                </template>
              </el-table-column>
            </el-table>
          </div>
          <div v-else-if="error" role="alert">
            {{ error }}
          </div>
          <div v-else>
            <el-table
                :data="paginate(getFilteredData(allEmployees), cur_page, page_size)"
                :default-sort="{prop: 'name', order: 'ascending'}"
                sortable="custom"
                style="width: 100%; margin-bottom: 20px;"
                row-key="id">
              <el-table-column
                  prop="name"
                  label="Сотрудник"
                  sortable
                  sort-by="name">
                <template slot-scope="scope">
                  <div v-if="clickable" style="cursor: pointer;" @click="clickedItem(scope.row.id)">{{ scope.row.name }}</div>
                  <div v-else>{{ scope.row.name }}</div>
                </template>
              </el-table-column>
              <el-table-column
                  prop="login"
                  width="150"
                  label="Логин"
                  align="right"
                  sortable
                  sort-by="login">
                <template slot-scope="scope">
                  {{ scope.row.login }}
                </template>
              </el-table-column>
              <el-table-column
                  prop="department"
                  width="150"
                  label="Отдел"
                  align="right"
                  sortable
                  sort-by="department">
                <template slot-scope="scope">
                  {{ scope.row.department }}
                </template>
              </el-table-column>
              <el-table-column
                  prop="role"
                  width="150"
                  label="Роль"
                  align="right"
                  sortable
                  sort-by="role">
                <template slot-scope="scope">
                  {{ scope.row.role }}
                </template>
              </el-table-column>
              <el-table-column v-if="showOptions"
                  label="Действие"
                  align="right"
                  width="100">
                <template slot-scope="scope">
                  <el-dropdown trigger="click">
                      <span class="el-dropdown-link">
                        <img src="@/assets/icons/more_vert.svg" alt="Действия">
                      </span>
                    <el-dropdown-menu slot="dropdown">
                      <el-dropdown-item v-if="isManager" @click.native="showResponsibilities(scope.row)">Показать ответственности</el-dropdown-item>
                      <el-dropdown-item v-if="isHR" @click.native="deleteReq(scope.row)">Удалить сотрудника</el-dropdown-item>
                    </el-dropdown-menu>
                  </el-dropdown>
                </template>
              </el-table-column>
            </el-table>
            <div style="float: right;">
              <el-pagination
                  small
                  :page-sizes="[5, 10, 25, 100]"
                  :page-size="page_size"
                  layout="sizes, prev, pager, next, slot"
                  :total="getFilteredData(allEmployees).length"
                  @size-change="handleSizeChange"
                  @current-change="handleCurrentChange">
              </el-pagination>
            </div>
          </div>
        </template>
      </async_employees>
    </section>
    <Modal_window v-if="showAddModal">
      <template v-slot:header>
        <h3 class="small-h3">Добавление сотрудника</h3>
      </template>
      <template v-slot:body>
        <el-form
            class="login-form"
            :model="model"
            :rules="rules"
            ref="form"
        >
          <el-form-item prop="inputLogin">
            <el-input
                v-model="model.inputLogin"
                placeholder="Логин сотрудника"/>
          </el-form-item>
          <el-form-item prop="inputRole">
            <el-select v-model="model.inputRole" placeholder="Select">
              <el-option
                  v-for="item in options"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value">
              </el-option>
            </el-select>
          </el-form-item>
        </el-form>
      </template>
      <template v-slot:footer>
        <div class="input-string">
          <el-button @click="closeAddModel" type="primary">Отмена</el-button>
          <el-button @click="validatedAddFormConfirm" type="primary" :loading="loading">Добавить</el-button>
        </div>
      </template>
    </Modal_window>
    <Modal_window v-if="showResponsiblesModal" :is-table="true">
      <template v-slot:header>
        <h3 class="small-h3">Ответственности пользователя</h3>
      </template>
      <template v-slot:body>
        <responsible_table :EmployeeId="curEmplId"></responsible_table>
      </template>
      <template v-slot:footer>
        <div class="input-string">
          <el-button @click="closeShowResponsibles" type="primary">Закрыть</el-button>
        </div>
      </template>
    </Modal_window>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapMutations } from "vuex";
import async_employees from '@/components/Async/Async_employees.vue';
import {addNewEmployee, removeEmployee} from "@/utils/requests/employees";
import Modal_window from '@/components/Modal.vue';
import responsible_table from '@/components/Responsibles_table.vue'
import {Employee} from "@/store/types";
import {ReceivedEmployee} from "@/store/incoming_types";

export default Vue.extend({
  name: "EmployeesTable",
  props: {
    getData: { type: Function, required: true },
    showOptions: { type: Boolean, required: true },
    clickable: { type: Boolean, required: false, default: false },
    clickedItem: { type: Function, required: false },
  },
  computed: {
    ...mapGetters([
      "allEmployees",
      "getCurId",
      "getUser",
      "isManager",
      "isHR"
    ])
  },
  data() {
    return {
      model: {
        inputLogin: "",
        inputRole: 0,
      },
      showAddModal: false,
      showResponsiblesModal: false,
      curEmplId: 0,
      options: [
        {
          value: 0,
          label: 'Employee'
        }, {
          value: 1,
          label: 'Responsible'
        }, {
          value: 2,
          label: 'Manager'
        }, {
          value: 3,
          label: 'HR'
        }, {
          value: 4,
          label: 'Founder'
        }],
      loading: false,
      search: "",
      multipleSelection: [] as number[],
      isSelected: false,
      page_size: 5,
      cur_page: 1,
      rules: {
        inputLogin: [
          {
            required: true,
            message: "Логин сотрудника - обязательно",
            trigger: "blur"
          }
        ],
        inputRole: [
          {
            required: true,
            message: "Роль сотрулника - обязательны",
            trigger: "blur"
          }
        ],
      }
    }
  },
  components: {
    Modal_window,
    async_employees,
    responsible_table,
  },
  methods: {
    ...mapMutations([
      "setCurId",
      "setUser"
    ]),
    requestItems() {
      return this.getData();
    },
    showResponsibilities(item: Employee) {
      this.curEmplId = item.id;
      this.showResponsiblesModal = true;
    },
    closeShowResponsibles() {
      this.showResponsiblesModal = false;
    },
    closeAddModel() {
      this.showAddModal = false;
    },
    giveMeAddModal() {
      this.showAddModal = true;
    },
    employeeAdd(item: ReceivedEmployee) {
      this.allEmployees.push({
        id: item.employeeid,
        login: this.model.inputLogin,
        name: item.name_ + ' ' + item.surname,
        department: item.department,
        role: (this.options.find(e => e.value === this.model.inputRole) as { value: number; label: string; }).label,
      });
      this.closeAddModel();
    },
    async validatedAddFormConfirm() {
      await this.postReq();
    },
    async postReq() {
      this.loading = true;

      await addNewEmployee(
          this.model.inputLogin,
          this.model.inputRole,
      )
          .then(response => {
            if (!response.ok) {
              return;
            }
            return response.json();
          })
          .then(item => {
            this.employeeAdd(item);
          })
          .catch((error) => {
            console.log("post:", error);
            this.closeAddModel();
          })
          .finally(() => {
            this.loading = false;
          })
    },
    async deleteReq(item: Employee) {
      await removeEmployee(item.id)
          .then(response => {
            if (!response.ok) {
              return;
            }
            this.employeeRemove(item);
          })
          .catch((error) => {
            console.log("delete:", error);
          })
    },
    employeeRemove(item: Employee) {
      let i = 0;
      while (i < this.allEmployees.length && this.allEmployees[i].id !== item.id) {
        i++;
      }
      if (i < this.allEmployees.length) {
        this.allEmployees.splice(i, 1);
      }
    },
    handleSelectionChange(val: number[]) {
      this.multipleSelection = val;
    },
    projectFilter(data: Employee) {
      return (!this.search || data.login.toLowerCase().includes(this.search.toLowerCase()));
    },
    getFilteredData(data: Employee[]) {
      return data.filter(data => this.projectFilter(data))
    },
    paginate(data: Employee[], page_number: number, page_size: number) {
      return data.slice((page_number - 1) * page_size, page_number * page_size);
    },
    async handleSizeChange(val: number) {
      this.page_size = val
    },
    async handleCurrentChange(val: number) {
      this.cur_page = val
    },
  },
});
</script>

<style scoped>

</style>
