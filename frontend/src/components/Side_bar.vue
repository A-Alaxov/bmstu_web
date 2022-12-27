<template>
  <div>
    <el-menu style="height: 100vh;"
        :default-active="activeIndex"
        :router="true"
        background-color="#F4F5FA"
        class="el-menu-vertical-demo">
      <el-menu-item-group>
        <el-menu-item index="userView">
          <i class="el-icon-user"></i>
        </el-menu-item>
        <el-menu-item index="workplaceSelection">
          <i class="el-icon-suitcase"></i>
        </el-menu-item>
      </el-menu-item-group>
      <el-menu-item-group v-if="getCurWorkplace != null">
        <el-menu-item index="workplaceView">
          <i class="el-icon-postcard"></i>
        </el-menu-item>
        <el-menu-item index="projects">
          <i class="el-icon-notebook-2"></i>
        </el-menu-item>
        <el-menu-item index="employeesView">
          <span class="material-icons">group</span>
        </el-menu-item>
        <el-menu-item index="departmentsView">
          <i class="el-icon-office-building"></i>
        </el-menu-item>
      </el-menu-item-group>
    </el-menu>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import {mapGetters} from "vuex";

export default Vue.extend({
  name: "sideBar",
  props: {
    activeIndex: { type: String, required: true },
  },
  computed: {
    ...mapGetters([
      "getCurWorkplace"
    ])
  },
});
</script>

<style scoped>
.el-menu-item span {
  color: #909399;
}

.el-menu-item.is-active span {
  color: inherit;
}
</style>
