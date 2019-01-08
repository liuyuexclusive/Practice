import Vue from 'vue'
import Router from 'vue-router'
import Layout from '@/views/Layout.vue'
import Login from '@/views/Login.vue'
import Register from '@/views/Register.vue'
import Test from '@/views/Test.vue'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name:'Login',
      component: Login
    },
    {
      path: '/Register',
      name:'Register',
      component: Register
    },
    {
      path: '/Layout',
      name: 'Layout',
      component: Layout
    },
    {
      path: '/',
      component: Layout,
      //iconCls: 'el-icon-message',//图标样式class
      children: [
        { path: '/Test',  name: 'Test',component: Test, meta: { requireAuth: true } },
      ]
    }
  ]
})
