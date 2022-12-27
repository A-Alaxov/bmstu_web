import {arrayToTree} from "performant-array-to-tree";
import {Objective} from "@/store/types";
import {ReceivedObjective} from "@/store/incoming_types";

export default {
  actions: {},
  state: {
    items: [] as Objective[],
  },
  mutations: {
    resetItems(state: {items: Objective[]}, items: ReceivedObjective[]) {
      items.sort((a, b) => {
        if (a.title > b.title) {
          return 1;
        }
        if (a.title < b.title) {
          return -1;
        }
        return 0;
      })
      const array = [] as Objective[];
      items.forEach(elem => array.push({
        id: elem.objectiveid,
        pid: elem.parentobjective,
        name: elem.title,
        company: elem.company,
        department: elem.department,
        startDate: elem.termbegin,
        endDate: elem.termend,
        estimatedTime: elem.estimatedtime,
        parent: null,
        children: []
      }))
      state.items = arrayToTree(array, { dataField: null, parentId: "pid" }) as Objective[];
    },
    updateItem(state: {items: Objective[]}, value: Objective) {
      state.items[value.id].name = value.name;
      state.items[value.id].startDate = value.startDate;
      state.items[value.id].endDate = value.endDate;
      state.items[value.id].estimatedTime = value.estimatedTime;
    },
  },
  getters: {
    allItems(state: {items: Objective[]}) {
      return state.items
    },
  },
}
