import Vue from 'vue'
import Router from 'vue-router'
import Role from '@/views/Role'
import RoleEdit from '@/views/RoleEdit'
import Test from '@/views/Test'
import Login from '@/views/Login.vue'
import Home from '@/views/Home.vue'
import Welcome from '@/views/Welcome.vue'
import Register from '@/views/Register.vue'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      component: Login,
      hidden: true
    },
    {
      path: '/Register',
      component: Register,
      hidden: true
    },
    {
      path: '/Home',
      component: Home,
      meta: { requireAuth: true, },
      hidden: true
    },
    {
      path: '/',
      component: Home,
      //iconCls: 'el-icon-message',//图标样式class
      children: [
        { path: '/Welcome', component: Welcome, name: 'Welcome', meta: { requireAuth: true } },
        { path: '/Role', component: Role, name: 'Role', meta: { requireAuth: true } },
        { path: '/RoleEdit', component: RoleEdit, name: 'RoleEdit', meta: { requireAuth: true } },
        { path: '/Test', component: Test, name: 'Test', meta: { requireAuth: true } },
      ]
    }
  ]
})
