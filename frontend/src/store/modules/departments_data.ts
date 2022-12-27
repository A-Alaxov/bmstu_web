import {Department} from "@/store/types";
import {ReceivedDepartment} from "@/store/incoming_types";

export default {
  actions: {},
  state: {
    departments: [] as Department[],
  },
  mutations: {
    resetDepartments(state: {departments: Department[]}, departments: ReceivedDepartment[]) {
      departments.sort((a, b) => {
        if (a.title > b.title) {
          return 1;
        }
        if (a.title < b.title) {
          return -1;
        }
        return 0;
      })
      const array = [] as Department[];
      departments.forEach(elem => array.push({
        id: elem.departmentid,
        title: elem.title,
        foundationyear: elem.foundationyear,
        activityfield: elem.activityfield,
      }));
      state.departments = array;
    },
    updateDepartment(state: {departments: Department[]}, value: Department) {
      state.departments[value.id].title = value.title;
      state.departments[value.id].foundationyear = value.foundationyear;
      state.departments[value.id].activityfield = value.activityfield;
    },
  },
  getters: {
    allDepartments(state: {departments: Department[]}) {
      return state.departments;
    },
  },
}
