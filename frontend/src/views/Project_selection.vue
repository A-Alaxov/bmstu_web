<template>
  <div>
    <header>
      <headerBar></headerBar>
    </header>
    <main>
      <sideBar active-index="projects"></sideBar>
      <div class="content">
        <section class="search">
          <div class="qs-container search__content-wrapper">
            <el-input type="search" placeholder="Поиск..." v-model="search">
              <i slot="prefix" class="el-input__icon el-icon-search"></i>
            </el-input>
            <button class="qs-icon-button" @click="showFilter = !showFilter">
              <img src="@/assets/icons/filter.svg" alt="Filter">
            </button>
            <el-button
                v-if="isManager"
                @click="giveMeAddModal"
                class="add-button"
                type="primary">
              Новый проект
            </el-button>
          </div>
          <transition name="fade" mode="out-in">
            <div class="qs-container" v-if="showFilter">
              <div class="input-string">
                <div class="date-group">
                  Дата начала
                  <div class="date-string">
                    <el-date-picker
                        v-model="startDateAfter"
                        placeholder="Не раньше"
                        type="date"
                        size="medium"
                        class="u-full"
                        value-format="yyyy-MM-dd"
                        style="margin-right: 15px"
                    ></el-date-picker>
                    <el-date-picker
                        v-model="startDateBefore"
                        placeholder="Не позже"
                        type="date"
                        size="medium"
                        class="u-full"
                        value-format="yyyy-MM-dd"
                    ></el-date-picker>
                  </div>
                </div>
                <div class="date-group">
                  Дата окончания
                  <div class="date-string">
                    <el-date-picker
                        v-model="endDateAfter"
                        placeholder="Не раньше"
                        type="date"
                        size="medium"
                        class="u-full"
                        value-format="yyyy-MM-dd"
                        style="margin-right: 15px"
                    ></el-date-picker>
                    <el-date-picker
                        v-model="endDateBefore"
                        placeholder="Не позже"
                        type="date"
                        size="medium"
                        class="u-full"
                        value-format="yyyy-MM-dd"
                    ></el-date-picker>
                  </div>
                </div>
              </div>
            </div>
          </transition>
        </section>
        <section class="goods">
          <async_items :request="requestItems">
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
                    :data="paginate(getFilteredData(allItems), cur_page, page_size)"
                    :default-sort="{prop: 'name', order: 'ascending'}"
                    sortable="custom"
                    style="width: 100%; margin-bottom: 20px;"
                    @selection-change="handleSelectionChange"
                    row-key="id">
                  <el-table-column
                      prop="name"
                      label="Название проекта"
                      sortable
                      sort-by="name">
                    <template slot-scope="scope">
                      <div style="cursor: pointer;" @click="handleSetCurId([scope.row.id])">{{ scope.row.name }}</div>
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
                      {{ timeFormat(scope.row.startDate, "D MMM YYYY") }}
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
                      {{ timeFormat(scope.row.endDate, "D MMM YYYY") }}
                    </template>
                  </el-table-column>
                  <el-table-column
                      prop="estimatedTime"
                      width="150"
                      label="Оставшееся время"
                      align="right"
                      sortable
                      sort-by="estimatedTime">
                    <template slot-scope="scope">
                      {{ timeSpanFormat(scope.row.estimatedTime) }}
                    </template>
                  </el-table-column>
                  <el-table-column
                      label="Действие"
                      align="right"
                      width="100">
                    <template slot-scope="scope">
                      <el-dropdown trigger="click">
                      <span class="el-dropdown-link">
                        <img src="@/assets/icons/more_vert.svg" alt="Действия">
                      </span>
                        <el-dropdown-menu slot="dropdown">
                          <el-dropdown-item v-if="isRespOrManager" @click.native="giveMeEditModal(scope.row)">Изменить проект</el-dropdown-item>
                          <el-dropdown-item v-if="isRespOrManager" @click.native="deleteReq(scope.row)">Удалить проект</el-dropdown-item>
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
                      :total="getFilteredData(allItems).length"
                      @size-change="handleSizeChange"
                      @current-change="handleCurrentChange">
                  </el-pagination>
                </div>
              </div>
            </template>
          </async_items>
        </section>
      </div>
    </main>
    <Modal_window v-if="showAddModal">
      <template v-slot:header>
        <h3 class="small-h3">Добавление проекта</h3>
      </template>
      <template v-slot:body>
        <el-form
            class="login-form"
            :model="model"
            :rules="rules"
            ref="form"
        >
          <el-form-item prop="inputName">
            <el-input
                v-model="model.inputName"
                placeholder="Название проекта"/>
          </el-form-item>
          <el-form-item prop="inputDates">
            <el-date-picker
                v-model="model.inputDates"
                type="daterange"
                size="medium"
                start-placeholder="Начало"
                end-placeholder="Конец"
                :clearable="false"
                value-format="yyyy-MM-dd"
            />
          </el-form-item>
          <el-form-item prop="estimatedTime">
            <el-input
                v-model="model.estimatedTime"
                placeholder="Оценоцное время"/>
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
    <Modal_window v-if="showEditModal">
      <template v-slot:header>
        <h3 class="small-h3">Изменение проекта</h3>
      </template>
      <template v-slot:body>
        <el-form
            class="login-form"
            :model="model"
            :rules="rules"
            ref="form"
        >
          <el-form-item prop="inputName">
            <el-input
                v-model="model.inputName"
                placeholder="Название проекта"/>
          </el-form-item>
          <el-form-item prop="inputDates">
            <el-date-picker
                v-model="model.inputDates"
                type="daterange"
                size="medium"
                start-placeholder="Начало"
                end-placeholder="Конец"
                :clearable="false"
                value-format="yyyy-MM-dd"
            />
          </el-form-item>
          <el-form-item prop="estimatedTime">
            <el-input
                v-model="model.estimatedTime"
                placeholder="Оценоцное время"/>
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
import { mapGetters, mapMutations } from "vuex";
import async_items from '@/components/Async/Async_items.vue';
import Modal_window from '@/components/Modal.vue';
import sideBar from '@/components/Side_bar.vue';
import dayjs from "dayjs";
import router from "../router";
import { addNewTask, getAllTasks, putTask, removeTask } from "@/utils/requests/tasks";
import headerBar from '@/components/Header_bar.vue';
import {Objective} from "@/store/types";

export default Vue.extend({
  name: "projectSelection",
  computed: {
    ...mapGetters([
      "allItems",
      "getCurWorkplace",
      "isManager",
      "isRespOrManager"
    ])
  },
  data() {
    return {
      model: {
        taskId: 0,
        inputName: "",
        inputDates: [] as Date[],
        estimatedTime: "",
      },
      loading: false,
      search: "",
      showAddModal: false,
      showEditModal: false,
      multipleSelection: [] as number[],
      startDateAfter: "",
      startDateBefore: "",
      endDateAfter: "",
      endDateBefore: "",
      isSelected: false,
      showFilter: false,
      page_size: 5,
      cur_page: 1,
      rules: {
        inputName: [
          {
            required: true,
            message: "Название проекта - обязательно",
            trigger: "blur"
          }
        ],
        inputDates: [
          {
            required: true,
            message: "Даты начала и конца - обязательны",
            trigger: "blur"
          }
        ],
        estimatedTime: [
          {
            required: true,
            message: "Оценочное время - обязательно",
            trigger: "blur"
          }
        ]
      }
    }
  },
  components: {
    headerBar,
    Modal_window,
    async_items,
    sideBar,
  },
  methods: {
    ...mapMutations([
      "updateItem",
      "setCurId",
    ]),
    requestItems() {
      return getAllTasks()
    },
    closeAddModel() {
      this.showAddModal = false;
    },
    giveMeAddModal() {
      this.showAddModal = true;
    },
    closeEditModel() {
      this.showEditModal = false;
    },
    giveMeEditModal(item: Objective) {
      this.model.taskId = item.id;
      this.model.inputName = item.name;
      this.model.inputDates[0] = item.startDate;
      this.model.inputDates[1] = item.endDate;
      this.model.estimatedTime = this.timeSpanFormat(item.estimatedTime);
      this.showEditModal = true;
    },
    taskAdd(itemId: number) {
      this.allItems.push({
        pid: null,
        id: itemId,
        name: this.model.inputName,
        _parent: null,
        startDate: this.model.inputDates[0],
        endDate: this.model.inputDates[1],
        estimatedTime: this.timeSpanFormatFromModel(this.model.estimatedTime),
        children: [],
      });
      this.closeAddModel();
    },
    passCheck(value: string) {
      if (value === '') {
        this.$notify({
          group: 'foo',
          type: 'error',
          text: 'Остаточное время пусто'
        });
      } else {
        if (!/(\d+d )?((0?\d)|(1\d)|(2[0-3]))h ([0-5]\dm )([0-5]\ds)/.test(value)) {
          this.$notify({
            group: 'foo',
            type: 'error',
            text: 'Формат: 0d 00h 00m 00s'
          });
          return false;
        } else {
          return true;
        }
      }
    },
    async validatedAddFormConfirm() {
      if (this.passCheck(this.model.estimatedTime)) {
        await this.postReq();
      }
    },
    async postReq() {
      this.loading = true;

      await addNewTask(
          this.model.inputName,
          (new Date(this.model.inputDates[0])).toISOString(),
          (new Date(this.model.inputDates[1])).toISOString(),
          this.formTicks(this.model.estimatedTime),
      )
          .then(response => {
            if (!response.ok) {
              return;
            }
            return response.json();
          })
          .then(task_id => {
            this.taskAdd(task_id);
          })
          .catch((error) => {
            console.log("post:", error);
            this.closeAddModel();
          })
      .finally(() => {
        this.loading = false;
      })
    },
    taskEdit() {
      let taskInd = this.allItems.findIndex((t: Objective) => t.id === this.model.taskId);
      let newItem = {
        id: taskInd,
        name: this.model.inputName,
        startDate: this.model.inputDates[0],
        endDate: this.model.inputDates[1],
        estimatedTime: this.timeSpanFormatFromModel(this.model.estimatedTime),
      }
      this.updateItem(newItem);
      this.closeEditModel();
    },
    async validatedEditFormConfirm() {
      await this.putReq();
    },
    async putReq() {
      this.loading = true;

      await putTask(
          this.model.taskId,
          this.model.inputName,
          (new Date(this.model.inputDates[0])).toISOString(),
          (new Date(this.model.inputDates[1])).toISOString(),
          this.formTicks(this.model.estimatedTime),
      )
          .then(() => {
            this.taskEdit();
          })
          .catch((error) => {
            console.log("put:", error);
            this.closeEditModel();
          })
          .finally(() => {
            this.loading = false;
          })
    },
    async deleteReq(item: Objective) {
      await removeTask(item.id)
        .then(response => {
          if (!response.ok) {
            return;
          }
          this.taskRemove(item);
        })
        .catch((error) => {
          console.log("delete:", error);
        })
    },
    taskRemove(item: Objective) {
      let curChildren;
      if (item.parent === null) {
        curChildren = this.allItems;
      }
      else {
        curChildren = item.parent.children;
      }
      let i = 0;
      while (i < curChildren.length && curChildren[i].id !== item.id) {
        i++;
      }
      if (i < curChildren.length) {
        curChildren.splice(i, 1);
      }
    },
    timeFormat(date: Date, format = "YYYY-MM-DD") {
      return date ? dayjs(date).format(format) : "";
    },
    timeSpanFormat(date: string) {
      return date ? date.replace('.', 'd ').replace(':', 'h ').replace(':', 'm ') + 's' : "";
    },
    timeSpanFormatFromModel(date: string) {
      return date ? date.replace('d ', '.').replace('h ', ':').replace('m ', ':').replace('s', '') : "";
    },
    formTicks(estTime: any) {
      let days = estTime.includes('d ') ? estTime.split('d ')[0] : 0;
      let hours = estTime.split('d ').at(-1).split('h ')[0];
      let minutes = estTime.split('d ').at(-1).split('h ').at(-1).split('m ')[0];
      let seconds = estTime.split('d ').at(-1).split('h ').at(-1).split('m ')[1].split('s')[0];
      let tmpDate = new Date(1000 * (Number(seconds)
          + 60 * (Number(minutes)
              + 60 * (Number(hours)
                  + 24 * Number(days)))));
      return tmpDate.getTime() * 10000;
    },
    handleSelectionChange(val: number[]) {
      this.multipleSelection = val;
    },
    projectFilter(data: Objective) {
      return (!this.search || data.name.toLowerCase().includes(this.search.toLowerCase())) &&
        (!this.startDateAfter || dayjs(data.startDate).isAfter(dayjs(this.startDateAfter))) &&
        (!this.startDateBefore || dayjs(data.startDate).isBefore(dayjs(this.startDateBefore))) &&
        (!this.endDateAfter || dayjs(data.endDate).isAfter(dayjs(this.endDateAfter))) &&
        (!this.endDateBefore || dayjs(data.endDate).isBefore(dayjs(this.endDateBefore)));
    },
    getFilteredData(data: Objective[]) {
      return data.filter(data => this.projectFilter(data))
    },
    paginate(data: Objective[], page_number: number, page_size: number) {
      return data.slice((page_number - 1) * page_size, page_number * page_size);
    },
    async handleSizeChange(val: number) {
      this.page_size = val
    },
    async handleCurrentChange(val: number) {
      this.cur_page = val
    },
    handleSetCurId(val: number) {
      this.setCurId(val);
      router.push('/projectView');
    },
  },
});
</script>

<style scoped>
@import '../assets/css/my_styles.css';
</style>
