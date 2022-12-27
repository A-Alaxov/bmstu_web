import {Workplace} from "@/store/types";
import {ReceivedWorkplace} from "@/store/incoming_types";

export default {
  actions: {},
  state: {
    workplaces: [] as Workplace[],
  },
  mutations: {
    resetWorkplaces(state: {workplaces: Workplace[]}, workplaces: ReceivedWorkplace[]) {
      workplaces.sort((a, b) => {
        if (a.permission_ > b.permission_) {
          return 1;
        }
        if (a.permission_ < b.permission_) {
          return -1;
        }
        return 0;
      })
      const array = [] as Workplace[];
      workplaces.forEach(elem => array.push({
        employee_id: elem.employeeID,
        company: elem.company.title,
        department: elem.department === null ? '' : elem.department.title,
        role: elem.permission_,
      }))
      state.workplaces = array;
    },
  },
  getters: {
    allWorkplaces(state: {workplaces: Workplace[]}) {
      return state.workplaces
    },
  },
}
