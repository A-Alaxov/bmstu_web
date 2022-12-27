import Vue from 'vue'
import Vuex from 'vuex'
import createPersistedState from 'vuex-persistedstate'
import user from './modules/user_data'
import curId from './modules/curId'
import item from './modules/item'
import workplace from './modules/workplace_data'
import employee from './modules/employees_data'
import department from './modules/departments_data'
import curWorkplace from './modules/cur_workplace'
import responsible from './modules/responsibles_data'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    user,
    curId,
    item,
    workplace,
    employee,
    department,
    curWorkplace,
    responsible,
  },
  plugins: [
    createPersistedState({
      paths: ['curId', 'user', 'curWorkplace'],
    }),
  ],
})
