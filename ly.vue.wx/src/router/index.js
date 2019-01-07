import Vue from 'vue'
import Router from 'vue-router'
import Layout from '@/views/Layout'
import Home from '@/views/Home'
import Trip from '@/views/Trip'
import Login from '@/views/Login'
import My from '@/views/My'
import Register from '@/views/Register'

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
      path: '/',
      component: Layout,
      //iconCls: 'el-icon-message',//图标样式class
      children: [
        { path: '/Home', name: 'Home', component: Home, meta: { requireAuth: true } },
        { path: '/Trip', name: 'Trip', component: Trip, meta: { requireAuth: true } },
        { path: '/My', name: 'My', component: My, meta: { requireAuth: true } }
      ]
    }
  ]
})
