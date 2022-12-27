export default {
  actions: {},
  state: {
    curId: 0,
  },
  mutations: {
    setCurId(state: {curId: number}, newCurId: number) {
      state.curId = newCurId;
    }
  },
  getters: {
    getCurId(state: {curId: number}) {
      return state.curId;
    },
  },
}
