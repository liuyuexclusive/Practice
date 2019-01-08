// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import MintUI from 'mint-ui'
import 'mint-ui/lib/style.css'
import i18n from './i18n/i18n'
import {store} from '@/vuex'

Vue.use(MintUI)

Vue.config.productionTip = false

router.beforeEach((to, from, next) => {
  //alert(to.name)
  if (to.matched.some(record => record.meta.requireAuth)){  // 判断该路由是否需要登录权限
    if (localStorage.token) {  // 判断当前的token是否存在 ； 登录存入的token
      store.state.currentMenuName=to.name;
      next();
    }
    else {
      next({
        path: '/',
        query: {redirect: to.fullPath}  // 将跳转的路由path作为参数，登录成功后跳转到该路由
      })
    }
  }
  else {
    store.state.currentMenuName=to.name;
    next();
  }
}
)

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  i18n,
  components: { App },
  template: '<App/>'
})
