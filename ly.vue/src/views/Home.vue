<template>
    <el-container style="height:100%;">
        <el-aside style="width:65px;border-right:1px solid #CFCFCF;">
            <navmenu></navmenu>
        </el-aside>
        <el-container>
            <el-header style="height:50px;padding:12px;border-bottom:1px solid #CFCFCF;">
                <el-row type="flex" class="row-bg">
                    <el-col :span="4">
                        <div class="grid-content bg-purple">{{currentMenuName}}</div>
                    </el-col>
                    <el-col :span="16">
                        <div class="grid-content bg-purple"></div>
                    </el-col>
                    <el-col :span="4" style="text-align:right;">
                        <div class="grid-content bg-purple">
                            <el-dropdown v-on:command="userHandler">
                                <span class="el-dropdown-link">
                                    {{username}}
                                    <i class="el-icon-arrow-down el-icon--right"></i>
                                </span>
                                <el-dropdown-menu slot="dropdown">
                                    <el-dropdown-item command="logoff">注销</el-dropdown-item>
                                    <el-dropdown-item command="setting">设置</el-dropdown-item>
                                </el-dropdown-menu>
                            </el-dropdown>
                        </div>
                    </el-col>
                </el-row>
            </el-header>
            <el-container>
                <div style="width:100%;padding:10px;">
                    <router-view></router-view>
                </div>
            </el-container>
        </el-container>
    </el-container>
</template>

<script>
import NavMenu from "@/components/NavMenu";
import { store } from "@/vuex";

export default {
  data() {
    return {
      name: "Home",
      username:
        localStorage.userName.length <= 18
          ? localStorage.userName
          : +localStorage.userName + "..."
    };
  },
  computed: {
    currentMenuName() {
      return store.state.currentMenuName;
    }
  },
  components: {
    navmenu: NavMenu
  },
  methods: {
    userHandler(cmd) {
      if (cmd === "logoff") {
        this.$confirm("确认注销？")
          .then(() => {
            localStorage.removeItem("token");
            this.$router.push({ name:"Login" });
          })
          .catch(() => {});
      }
    },
    test(){
        alert(11)
    }
  }
};
</script>
