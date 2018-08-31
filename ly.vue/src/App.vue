<template>
  <div id="app" style="100%">
    <el-container>
      <el-header style="height: 50px;padding:12px;z-index:9999">
        <el-row type="flex" class="row-bg">
          <el-col :span="2">
            <div class="grid-content bg-purple">{{this.$t('message.hello')}}</div>
          </el-col>
          <el-col :span="16">
            <div class="grid-content bg-purple"></div>
          </el-col>
          <el-col :span="6" style="text-align:right;">
            <div class="grid-content bg-purple">
              <el-dropdown split-button>
                ly
                <el-dropdown-menu slot="dropdown">
                  <el-dropdown-item>注销</el-dropdown-item>
                  <el-dropdown-item>设置</el-dropdown-item>
                </el-dropdown-menu>
              </el-dropdown>
            </div>
          </el-col>
        </el-row>
      </el-header>
      <el-container>
        <el-aside style="width: 200px">
          <navmenu></navmenu>
        </el-aside>
        <el-container>
          <el-main>
            <div>
              <router-view></router-view>
            </div>  
          </el-main>
          <!-- <el-footer></el-footer> -->
        </el-container>
      </el-container>
    </el-container>

    <!-- {{ $t("message.hello") }} -->
    <!-- <img src="./assets/logo.png"> -->
    <!-- <router-view/> -->
  </div>
</template>

<script>
import NavMenu from "@/components/NavMenu";

export default {
  name: "App",
  components: {
    navmenu: NavMenu
  }
};
</script>
