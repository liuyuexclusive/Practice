 <template>
  <div>
    <el-row type="flex" class="row-bg">
      <!-- <el-button @click="add">新增</el-button>
      <el-button @click="update">修改</el-button>
      <el-button @click="remove">删除</el-button> -->
      <el-button @click="refresh">刷新</el-button>
    </el-row>
    <pageTable ref="pageTable" :columnData="columnData" getListUrl="User/GetList" deleteUrl="User/delete"></pageTable>
  </div>
</template>

<script>
import PageTable from "@/components/PageTable";
export default {
  data() {
    return {
      columnData: [
        { prop: "Name", label: "名称" },
        { prop: "Email", label: "邮箱" },
        { prop: "Mobile", label: "电话" },
        { prop: "RoleName", label: "角色" },
        { prop: "LastOn", label: "最后登录时间" }
      ]
    };
  },
  components: {
    pageTable: PageTable
  },
  methods: {
    refresh() {
      this.$refs.pageTable.refresh();
    },
    remove() {
      this.$refs.pageTable.remove();
    }
  }
};
</script>