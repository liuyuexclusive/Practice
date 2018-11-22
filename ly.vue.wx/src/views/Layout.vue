<template>
    <div>
        <x-header :title="title"></x-header>
        <router-view></router-view>
        <tabbar @on-index-change="indexChange">
            <tabbar-item v-for="item in items" :selected="item.selected" :link="item.link" :badge="item.badge" :show-dot="item.showdot">
                <img slot="icon" :src="item.src">
                <span slot="label">{{item.title}}</span>
            </tabbar-item>
        </tabbar>
    </div>
</template>

<script>
import { Tabbar, TabbarItem,  XHeader } from "vux";

export default {
  data() {
    return {
      title:"",
      items: [
        {
          title:"首页",
          link: "/",
          src: require("vux/src/icons/home.svg"),
          showdot: false,
          badge: "",
          selected: true
        },
        {
          title:"行程",
          link: "/Trip",
          src: require("vux/src/icons/plane.svg"),
          showdot: true,
          badge: "",
          selected: false
        },
        {
          title:"我的",
          link: "/My",
          src: require("vux/src/icons/person.svg"),
          showdot: true,
          badge: "2",
          selected: false
        }
      ]
    };
  },
  components: {
    Tabbar,
    TabbarItem,
    XHeader
  },
  methods:{
      indexChange(index){
          this.title = this.items[index].title;
      }
  }
};
</script>

<style>
.vux-x-icon {
  fill: #f70968;
}
</style>