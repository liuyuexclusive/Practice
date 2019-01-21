import Vue from 'vue'
import Router from 'vue-router'
import Role from '@/views/sys/Role'
import RoleEdit from '@/views/sys/RoleEdit'
import User from '@/views/sys/User'
import Login from '@/views/Login.vue'
import Home from '@/views/Home.vue'
import Welcome from '@/views/Welcome.vue'
import Register from '@/views/Register.vue'
import Test from '@/views/sys/Test.vue'
import WorkflowType from '@/views/sys/WorkflowType.vue'
import WorkflowTypeEdit from '@/views/sys/WorkflowTypeEdit.vue'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name:'Login',
      component: Login,
      hidden: true
    },
    {
      path: '/Register',
      name:'Register',
      component: Register,
      hidden: true
    },
    {
      path: '/Home',
      name:'Home',
      component: Home,
      meta: { requireAuth: true, },
      hidden: true
    },
    {
      path: '/',
      component: Home,
      //iconCls: 'el-icon-message',//图标样式class
      children: [
        { path: '/Welcome',  name: 'Welcome',component: Welcome, meta: { requireAuth: true } },
        { path: '/Role', name: 'Role', component: Role, meta: { requireAuth: true } },
        { path: '/RoleEdit', name: 'RoleEdit', component: RoleEdit, meta: { requireAuth: true } },
        { path: '/User', name: 'User', component: User, meta: { requireAuth: true } },
        { path: '/WorkflowType', name: 'WorkflowType', component: WorkflowType, meta: { requireAuth: true } },
        { path: '/WorkflowTypeEdit', name: 'WorkflowTypeEdit', component: WorkflowTypeEdit, meta: { requireAuth: true } },
        { path: '/Test', name: 'Test', component: Test, meta: { requireAuth: true } },
      ]
    }
  ]
})
