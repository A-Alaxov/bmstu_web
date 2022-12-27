<template>
  <div>
    <header>
      <headerBar></headerBar>
    </header>
    <main>
      <sideBar active-index="employeesView"></sideBar>
      <div class="content">
        <Employees_table
            :get-data="requestItems"
            :show-options="true"
        ></Employees_table>
      </div>
    </main>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapMutations } from "vuex";
import Employees_table from '@/components/Employees_table.vue';
import sideBar from '@/components/Side_bar.vue';
import { getAllEmployees } from "@/utils/requests/employees";
import headerBar from '@/components/Header_bar.vue';

export default Vue.extend({
  name: "employeesView",
  computed: {
    ...mapGetters([
      "allEmployees",
      "getCurId",
    ])
  },
  components: {
    sideBar,
    Employees_table,
    headerBar,
  },
  methods: {
    ...mapMutations([
      "setCurId",
    ]),
    requestItems() {
      return getAllEmployees()
    },
  },
});
</script>

<style scoped>
@import '../assets/css/my_styles.css';
</style>
