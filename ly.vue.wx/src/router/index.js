import Vue from 'vue'
import Router from 'vue-router'
import Layout from '@/views/Layout'
import Home from '@/views/Home'
import Trip from '@/views/Trip'
import My from '@/views/My'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      component: Layout,
      //iconCls: 'el-icon-message',//图标样式class
      children: [
        { path: '/',  name: 'Home',component: Home},
        { path: '/Trip', name: 'Role', component: Trip},
        { path: '/My', name: 'My', component: My}
      ]
    }
  ]
})
