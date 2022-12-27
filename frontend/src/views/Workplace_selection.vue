<template>
  <div>
    <header>
      <headerBar></headerBar>
    </header>
    <main>
      <sideBar active-index="workplaceSelection"></sideBar>
      <div class="content">
        <section class="search">
          <div class="qs-container search__content-wrapper">
            <el-button
                @click="giveMeAddModal"
                class="add-button"
                type="primary">
              Новая компания
            </el-button>
          </div>
        </section>
        <section class="goods">
          <async_workplaces :request="requestItems">
            <template v-slot:default="{ pending, error }">
              <div v-if="pending" class="text-center">
                <el-table
                    v-loading="true"
                    :default-sort="{prop: 'name', order: 'ascending'}"
                    sortable="custom"
                    style="width: 100%; margin-bottom: 20px;"
                    @selection-change="handleSelectionChange"
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
                      <router-link style="color: black" :to="{ name: 'GanttDiagram' }" @click.native="handleSetWorkplaceId([scope.row.id])">{{ scope.row.name }}</router-link>
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
                    :data="paginate(allWorkplaces, cur_page, page_size)"
                    :default-sort="{prop: 'role', order: 'ascending'}"
                    sortable="custom"
                    style="width: 100%; margin-bottom: 20px;"
                    @selection-change="handleSelectionChange"
                    row-key="id">
                  <el-table-column
                      prop="role"
                      label="Role"
                      sortable
                      sort-by="role">
                    <template slot-scope="scope">
                      <div style="cursor: pointer;" @click="handleSetWorkplaceId([scope.row.employee_id])">{{ scope.row.role }}</div>
                    </template>
                  </el-table-column>
                  <el-table-column
                      prop="company"
                      width="150"
                      label="Company"
                      align="right"
                      sortable
                      sort-by="company">
                    <template slot-scope="scope">
                      <div style="cursor: pointer;" @click="handleSetWorkplaceId([scope.row.employee_id])">{{ scope.row.company }}</div>
                    </template>
                  </el-table-column>
                  <el-table-column
                      prop="department"
                      width="150"
                      label="Department"
                      align="right"
                      sortable
                      sort-by="department">
                    <template slot-scope="scope">
                      <div style="cursor: pointer;" @click="handleSetWorkplaceId([scope.row.employee_id])">{{ scope.row.department }}</div>
                    </template>
                  </el-table-column>
                </el-table>
                <div style="float: right;">
                  <el-pagination
                      small
                      :page-sizes="[5, 10, 25, 100]"
                      :page-size="page_size"
                      layout="sizes, prev, pager, next, slot"
                      :total="allWorkplaces.length"
                      @size-change="handleSizeChange"
                      @current-change="handleCurrentChange">
                  </el-pagination>
                </div>
              </div>
            </template>
          </async_workplaces>
        </section>
      </div>
    </main>
    <Modal_window v-if="showAddModal">
      <template v-slot:header>
        <h3 class="small-h3">Добавление компании</h3>
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
          <el-button @click="closeAddModel" type="primary">Отмена</el-button>
          <el-button @click="validatedAddFormConfirm" type="primary" :loading="loading">Добавить</el-button>
        </div>
      </template>
    </Modal_window>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapMutations } from "vuex";
import async_workplaces from '@/components/Async/Async_workplaces.vue';
import sideBar from '@/components/Side_bar.vue';
import router from "../router";
import { getAllWorkplaces, chooseWorkplace } from "@/utils/requests/workplaces";
import { addNewCompany } from "@/utils/requests/company";
import headerBar from '@/components/Header_bar.vue';
import Modal_window from '@/components/Modal.vue';
import {Workplace} from "@/store/types";
import {ReceivedWorkplace} from "@/store/incoming_types";

export default Vue.extend({
  name: "workplaceSelection",
  computed: {
    ...mapGetters([
      "allWorkplaces",
    ])
  },
  data() {
    return {
      model: {
        inputTitle: "",
        inputFoundationYear: 0,
      },
      showAddModal: false,
      loading: false,
      multipleSelection: [] as number[],
      isSelected: false,
      showFilter: false,
      page_size: 5,
      cur_page: 1,
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
    async_workplaces,
    sideBar,
    headerBar,
    Modal_window,
  },
  methods: {
    ...mapMutations([
      "setCurWorkplace",
    ]),
    requestItems() {
      return getAllWorkplaces()
    },
    closeAddModel() {
      this.showAddModal = false;
    },
    giveMeAddModal() {
      this.showAddModal = true;
    },
    workplaceAdd(item: ReceivedWorkplace) {
      this.allWorkplaces.push({
        employee_id: item.employeeID,
        company: item.company.title,
        department: item.department === null ? '' : item.department.title,
        role: item.permission_,
      });
      this.closeAddModel();
    },
    async validatedAddFormConfirm() {
      await this.postReq();
    },
    async postReq() {
      this.loading = true;

      await addNewCompany(
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
            this.workplaceAdd(item);
          })
          .catch((error) => {
            console.log("post:", error);
            this.closeAddModel();
          })
          .finally(() => {
            this.loading = false;
          })
    },
    handleSelectionChange(val: number[]) {
      this.multipleSelection = val;
    },
    paginate(data: Workplace[], page_number: number, page_size: number) {
      return data.slice((page_number - 1) * page_size, page_number * page_size);
    },
    async handleSizeChange(val: number) {
      this.page_size = val;
    },
    async handleCurrentChange(val: number) {
      this.cur_page = val;
    },
    async handleSetWorkplaceId(val: number) {
      await chooseWorkplace(val)
        .then(response => {
          if (!response.ok)
            throw new Error("Что-то пошло не так");

          return response.json();
        })
        .then(item => {
          this.setCurWorkplace(item);
          router.push('/projects');
        })
    }
  },
});
</script>

<style scoped>
@import '../assets/css/my_styles.css';
</style>
