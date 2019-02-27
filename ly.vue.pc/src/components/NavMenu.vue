<template>
    <el-menu style="border:none" router :default-active="$route.path" :default-openeds="['1']" default-active="/Welcome" :collapse="isCollapse">
        <el-submenu index="1">
            <template slot="title">
                <i class="el-icon-setting"></i>
            </template>
            <el-menu-item-group>
                <span slot="title">设置</span>
                <el-menu-item index="/Welcome">Welcome</el-menu-item>
                <el-menu-item index="/Role">Role</el-menu-item>
                <el-menu-item index="/User">User</el-menu-item>
                <el-menu-item index="/WorkflowType">WorkflowType</el-menu-item>
                <el-menu-item index="/Test">Test</el-menu-item>
                <el-menu-item index="/Schedule">Schedule</el-menu-item>
                <el-submenu index="1-4">
                    <template slot="title">选项4</template>
                    <el-menu-item index="1-4-1">选项4-1</el-menu-item>
                </el-submenu>
            </el-menu-item-group>
        </el-submenu>
        <el-submenu index="2">
            <template slot="title">
                <i class="el-icon-menu"></i>
                <span slot="title">导航二</span>
            </template>
            <el-menu-item index="2-1">选项1</el-menu-item>
            <el-menu-item index="2-2">选项2</el-menu-item>
            <el-menu-item index="2-3">选项3</el-menu-item>
            <el-submenu index="2-4">
                <template slot="title">选项4</template>
                <el-menu-item index="2-4-1">选项4-1</el-menu-item>
            </el-submenu>
        </el-submenu>
        <el-submenu index="3">
            <template slot="title">
                <i class="el-icon-message"></i>
                <span slot="title"> 导航三</span>
            </template>
            <el-menu-item index="3-1">选项1</el-menu-item>
            <el-menu-item index="3-2">选项2</el-menu-item>
            <el-menu-item index="3-3">选项3</el-menu-item>
            <el-submenu index="3-4">
                <template slot="title">选项4</template>
                <el-menu-item index="3-4-1">选项4-1</el-menu-item>
            </el-submenu>
            <el-menu-item index="3-5">选项5</el-menu-item>
            <el-menu-item index="3-6">选项6</el-menu-item>
            <el-menu-item index="3-7">选项7</el-menu-item>
            <el-menu-item index="3-8">选项8</el-menu-item>
            <el-menu-item index="3-9">选项9</el-menu-item>
            <el-menu-item index="3-9">选项9</el-menu-item>
        </el-submenu>
    </el-menu>
</template>

<script>
export default {
  name: "NavMenu",
  data() {
    return {
      isCollapse: true
    };
  }
};
</script>