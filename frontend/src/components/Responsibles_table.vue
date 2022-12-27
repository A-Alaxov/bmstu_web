<template>
  <div>
    <div class="content">
      <section class="search">
        <div class="qs-container search__content-wrapper">
          <el-input type="search" placeholder="Поиск..." v-model="search">
            <i slot="prefix" class="el-input__icon el-icon-search"></i>
          </el-input>
        </div>
      </section>
      <section class="goods">
        <async_responsibilities :request="requestItems">
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
                  :data="paginate(getFilteredData(allResponsibles), cur_page, page_size)"
                  :default-sort="{prop: 'task', order: 'ascending'}"
                  sortable="custom"
                  style="width: 100%; margin-bottom: 20px;"
                  row-key="id">
                <el-table-column
                    prop="task"
                    label="Задача"
                    sortable
                    sort-by="task">
                  <template slot-scope="scope">
                    {{ scope.row.task }}
                  </template>
                </el-table-column>
                <el-table-column
                    prop="timespent"
                    width="150"
                    label="Потраченное время"
                    align="right"
                    sortable
                    sort-by="timespent">
                  <template slot-scope="scope">
                    {{ timeSpanFormat(scope.row.timespent) }}
                  </template>
                </el-table-column>
              </el-table>
              <div style="float: right;">
                <el-pagination
                    small
                    :page-sizes="[5, 10, 25, 100]"
                    :page-size="page_size"
                    layout="sizes, prev, pager, next, slot"
                    :total="getFilteredData(allResponsibles).length"
                    @size-change="handleSizeChange"
                    @current-change="handleCurrentChange">
                </el-pagination>
              </div>
            </div>
          </template>
        </async_responsibilities>
      </section>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from "vuex";
import async_responsibilities from '@/components/Async/Async_responsibilities.vue';
import {getEmplResponsibilities} from "@/utils/requests/employees";
import {Responsibility} from "@/store/types";

export default Vue.extend({
  name: "responsible_table",
  props: {
    EmployeeId: { type: Number, required: true },
  },
  computed: {
    ...mapGetters([
      "allResponsibles",
      "getCurId",
    ])
  },
  data() {
    return {
      loading: false,
      search: "",
      multipleSelection: [] as number[],
      isSelected: false,
      page_size: 5,
      cur_page: 1,
    }
  },
  components: {
    async_responsibilities,
  },
  methods: {
    requestItems() {
      return getEmplResponsibilities(this.EmployeeId);
    },
    timeSpanFormat(date: string) {
      return date ? date.replace('.', 'd ').replace(':', 'h ').replace(':', 'm ') + 's' : "";
    },
    handleSelectionChange(val: number[]) {
      this.multipleSelection = val;
    },
    projectFilter(data: Responsibility) {
      return (!this.search || data.task.toLowerCase().includes(this.search.toLowerCase()));
    },
    getFilteredData(data: Responsibility[]) {
      return data.filter(data => this.projectFilter(data))
    },
    paginate(data: Responsibility[], page_number: number, page_size: number) {
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
