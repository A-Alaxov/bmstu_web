export default {
  actions: {},
  state: {
    user: "",
  },
  mutations: {
    setUser(state: {user: string}, newUser: any) {
      state.user = JSON.stringify(newUser);
    },
  },
  getters: {
    getUser(state: {user: string}) {
      return JSON.parse(state.user);
    },
  },
}
