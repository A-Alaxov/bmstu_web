import {Employee} from "@/store/types";
import {ReceivedEmployee} from "@/store/incoming_types";

export default {
  actions: {},
  state: {
    employees: [] as Employee[],
  },
  mutations: {
    resetEmployees(state: {employees: Employee[]}, employees: ReceivedEmployee[]) {
      employees.sort((a, b) => {
        if (a.name_ > b.name_) {
          return 1;
        }
        if (a.name_ < b.name_) {
          return -1;
        }
        return 0;
      })
      const array = [] as Employee[];
      employees.forEach(elem => array.push({
        id: elem.employeeid,
        login: elem.login,
        name: elem.name_ + ' ' + elem.surname,
        department: elem.department,
        role: elem.permission_,
      }))
      state.employees = array;
    },
  },
  getters: {
    allEmployees(state: {employees: Employee[]}) {
      return state.employees
    },
  },
}
