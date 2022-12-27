import {Responsibility} from "@/store/types";
import {ReceivedResponsibility} from "@/store/incoming_types";

export default {
  actions: {},
  state: {
    responsibles: [] as Responsibility[],
  },
  mutations: {
    resetResponsibles(state: {responsibles: Responsibility[]}, responsibles: ReceivedResponsibility[]) {
      responsibles.sort((a, b) => {
        if (a.objective > b.objective) {
          return 1;
        }
        if (a.objective < b.objective) {
          return -1;
        }
        return 0;
      })
      const array = [] as Responsibility[];
      responsibles.forEach(elem => array.push({
        employee: elem.employee,
        task: elem.objective,
        timespent: elem.timespent,
      }));
      state.responsibles = array;
    },
  },
  getters: {
    allResponsibles(state: {responsibles: Responsibility[]}) {
      return state.responsibles;
    },
  },
}
