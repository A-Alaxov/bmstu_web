export default {
  actions: {},
  state: {
    curWorkplace: ""
  },
  mutations: {
    setCurWorkplace(state: {curWorkplace: string}, newWorkplace: any) {
      state.curWorkplace = JSON.stringify(newWorkplace);
    }
  },
  getters: {
    getCurWorkplace(state: {curWorkplace: string}) {
      return JSON.parse(state.curWorkplace);
    },
    isFounder(state: {curWorkplace: string}) {
      const tmp = JSON.parse(state.curWorkplace);
      return tmp.permission_ === "Founder";
    },
    isResponsible(state: {curWorkplace: string}) {
      const tmp = JSON.parse(state.curWorkplace);
      return tmp.permission_ === "Responsible";
    },
    isManager(state: {curWorkplace: string}) {
      const tmp = JSON.parse(state.curWorkplace);
      return tmp.permission_ === "Manager" || tmp.permission_ === "Founder";
    },
    isHR(state: {curWorkplace: string}) {
      const tmp = JSON.parse(state.curWorkplace);
      return tmp.permission_ === "HR" || tmp.permission_ === "Founder";
    },
    isRespOrManager(state: {curWorkplace: string}) {
      const tmp = JSON.parse(state.curWorkplace);
      return tmp.permission_ === "Manager" || tmp.permission_ === "Founder" || tmp.permission_ === "Responsible";
    },
    isAboveEmployee(state: {curWorkplace: string}) {
      const tmp = JSON.parse(state.curWorkplace);
      return tmp.permission_ === "Manager" || tmp.permission_ === "Founder" || tmp.permission_ === "Responsible" || tmp.permission_ === "HR";
    }
  },
}
