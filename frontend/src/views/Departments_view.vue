<template>
  <div>
    <header>
      <headerBar></headerBar>
    </header>
    <main>
      <sideBar active-index="departmentsView"></sideBar>
      <div class="content">
        <section class="search">
          <div class="qs-container search__content-wrapper">
            <el-input type="search" placeholder="Поиск..." v-model="search">
              <i slot="prefix" class="el-input__icon el-icon-search"></i>
            </el-input>
            <el-button
               v-if="isManager"
               @click="giveMeAddModal"
               class="add-button"
               type="primary">
              Новый отдел
            </el-button>
          </div>
        </section>
        <section class="goods">
          <async_departments :request="requestItems">
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
                    :data="paginate(getFilteredData(allDepartments), cur_page, page_size)"
                    :default-sort="{prop: 'title', order: 'ascending'}"
                    sortable="custom"
                    style="width: 100%; margin-bottom: 20px;"
                    row-key="id">
                  <el-table-column
                      prop="title"
                      label="Название"
                      sortable
                      sort-by="title">
                    <template slot-scope="scope">
                      {{ scope.row.title }}
                    </template>
                  </el-table-column>
                  <el-table-column
                      prop="foundationyear"
                      width="150"
                      label="Год основания"
                      align="right"
                      sortable
                      sort-by="foundationyear">
                    <template slot-scope="scope">
                      {{ scope.row.foundationyear }}
                    </template>
                  </el-table-column>
                  <el-table-column
                      prop="activityfield"
                      width="150"
                      label="Область деятельности"
                      align="right"
                      sortable
                      sort-by="activityfield">
                    <template slot-scope="scope">
                      {{ scope.row.activityfield }}
                    </template>
                  </el-table-column>
                  <el-table-column v-if="isManager"
                      label="Действие"
                      align="right"
                      width="100">
                    <template slot-scope="scope">
                      <el-dropdown trigger="click">
                      <span class="el-dropdown-link">
                        <img src="@/assets/icons/more_vert.svg" alt="Действия">
                      </span>
                        <el-dropdown-menu slot="dropdown">
                          <el-dropdown-item @click.native="giveMeEditModal(scope.row)">Изменить отдел</el-dropdown-item>
                          <el-dropdown-item @click.native="deleteReq(scope.row)">Удалить отдел</el-dropdown-item>
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
                      :total="getFilteredData(allDepartments).length"
                      @size-change="handleSizeChange"
                      @current-change="handleCurrentChange">
                  </el-pagination>
                </div>
              </div>
            </template>
          </async_departments>
        </section>
      </div>
    </main>
    <Modal v-if="showAddModal">
      <template v-slot:header>
        <h3 class="small-h3">Добавление отдела</h3>
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
                placeholder="Название отдела"/>
          </el-form-item>
          <el-form-item prop="inputFoundationYear">
            <el-input
                v-model="model.inputFoundationYear"
                placeholder="Год основания"/>
          </el-form-item>
          <el-form-item prop="inputRole">
            <el-input
                v-model="model.inputActivityField"
                placeholder="Область деятельности"/>
          </el-form-item>
        </el-form>
      </template>
      <template v-slot:footer>
        <div class="input-string">
          <el-button @click="closeAddModel" type="primary">Отмена</el-button>
          <el-button @click="validatedAddFormConfirm" type="primary" :loading="loading">Добавить</el-button>
        </div>
      </template>
    </Modal>
    <Modal v-if="showEditModal">
      <template v-slot:header>
        <h3 class="small-h3">Изменение отдела</h3>
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
                placeholder="Название отдела"/>
          </el-form-item>
          <el-form-item prop="inputFoundationYear">
            <el-input
                v-model="model.inputFoundationYear"
                placeholder="Год основания"/>
          </el-form-item>
          <el-form-item prop="inputRole">
            <el-input
                v-model="model.inputActivityField"
                placeholder="Область деятельности"/>
          </el-form-item>
        </el-form>
      </template>
      <template v-slot:footer>
        <div class="input-string">
          <el-button @click="closeEditModel" type="primary">Отмена</el-button>
          <el-button @click="validatedEditFormConfirm" type="primary" :loading="loading">Изменить</el-button>
        </div>
      </template>
    </Modal>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapMutations } from "vuex";
import Modal from '@/components/Modal.vue';
import sideBar from '@/components/Side_bar.vue';
import {addNewDepartment, getAllDepartments, putDepartment, removeDepartment} from "@/utils/requests/departments";
import headerBar from '@/components/Header_bar.vue';
import async_departments from '@/components/Async/Async_departments.vue';
import {Department} from "@/store/types";

export default Vue.extend({
  name: "departmentsView",
  computed: {
    ...mapGetters([
      "allDepartments",
      "getCurId",
      "isManager"
    ])
  },
  data() {
    return {
      model: {
        inputTitle: "",
        inputFoundationYear: 0,
        inputActivityField: "",
      },
      curDepId: 0,
      loading: false,
      search: "",
      showAddModal: false,
      showEditModal: false,
      multipleSelection: [] as number[],
      isSelected: false,
      page_size: 5,
      cur_page: 1,
      rules: {
        inputTitle: [
          {
            required: true,
            message: "Название отдела - обязательно",
            trigger: "blur"
          }
        ],
        inputFoundationYear: [
          {
            required: true,
            message: "Год основания - обязательны",
            trigger: "blur"
          }
        ],
        inputActivityField: [
          {
            required: true,
            message: "Область деятельности - обязательны",
            trigger: "blur"
          }
        ],
      }
    }
  },
  components: {
    Modal,
    sideBar,
    headerBar,
    async_departments,
  },
  methods: {
    ...mapMutations([
      "setCurId",
      "updateDepartment",
    ]),
    requestItems() {
      return getAllDepartments()
    },
    giveMeEditModal(item: Department) {
      this.curDepId = item.id;
      this.model.inputTitle = item.title;
      this.model.inputFoundationYear = item.foundationyear;
      this.model.inputActivityField = item.activityfield;
      this.showEditModal = true;
    },
    closeEditModel() {
      this.showEditModal = false;
    },
    closeAddModel() {
      this.showAddModal = false;
    },
    giveMeAddModal() {
      this.showAddModal = true;
    },
    departmentAdd(itemId: number) {
      this.allDepartments.push({
        id: itemId,
        title: this.model.inputTitle,
        foundationyear: this.model.inputFoundationYear,
        activityfield: this.model.inputActivityField,
      });
      this.closeAddModel();
    },
    async validatedAddFormConfirm() {
      await this.postReq();
    },
    async postReq() {
      this.loading = true;

      await addNewDepartment(
          this.model.inputTitle,
          this.model.inputFoundationYear,
          this.model.inputActivityField,
      )
          .then(response => {
            if (!response.ok) {
              return;
            }
            return response.json();
          })
          .then(task_id => {
            this.departmentAdd(task_id);
          })
          .catch((error) => {
            console.log("post:", error);
            this.closeAddModel();
          })
      .finally(() => {
        this.loading = false;
      })
    },
    depEdit() {
      let depInd = this.allDepartments.findIndex((t: Department) => t.id === this.curDepId);
      let newDep = {
        id: depInd,
        title: this.model.inputTitle,
        foundationyear: this.model.inputFoundationYear,
        activityfield: this.model.inputActivityField,
      }
      this.updateDepartment(newDep);
      this.closeEditModel();
    },
    async validatedEditFormConfirm() {
      await this.putReq();
    },
    async putReq() {
      this.loading = true;

      await putDepartment(
          this.curDepId,
          this.model.inputTitle,
          this.model.inputFoundationYear,
          this.model.inputActivityField,
      )
          .then(() => {
            this.depEdit();
          })
          .catch((error) => {
            console.log("put:", error);
            this.closeEditModel();
          })
          .finally(() => {
            this.loading = false;
          })
    },
    async deleteReq(item: Department) {
      await removeDepartment(item.id)
        .then(response => {
          if (!response.ok) {
            return;
          }
          this.departmentRemove(item);
        })
        .catch((error) => {
          console.log("delete:", error);
        })
    },
    departmentRemove(item: Department) {
      let i = 0;
      while (i < this.allDepartments.length && this.allDepartments[i].id !== item.id) {
        i++;
      }
      if (i < this.allDepartments.length) {
        this.allDepartments.splice(i, 1);
      }
    },
    handleSelectionChange(val: number[]) {
      this.multipleSelection = val;
    },
    projectFilter(data: Department) {
      return (!this.search || data.title.toLowerCase().includes(this.search.toLowerCase()));
    },
    getFilteredData(data: Department[]) {
      return data.filter(data => this.projectFilter(data))
    },
    paginate(data: Department[], page_number: number, page_size: number) {
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
@import '../assets/css/my_styles.css';
</style>
