import Vue from 'vue';
import Vuex from 'vuex'
Vue.use(Vuex)

export const store = new Vuex.Store({
  state: {
    currentMenuName: "",
    pageSizes: [30, 60, 90, 120, 150],
    pageSize: 30
  },
  methods: {
    test() {
      alert(1)
    }
  }
})