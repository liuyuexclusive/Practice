 <template>
  <div>
    <el-row type="flex" class="row-bg">
      <el-button @click="add">新增</el-button>
      <el-button @click="update">修改</el-button>
      <el-button @click="remove">删除</el-button>
      <el-button @click="refresh">刷新</el-button>
    </el-row>
    <pageTable ref="pageTable" :columnData="columnData" getListUrl="Role/GetList" deleteUrl="Role/delete" editFormRouteName="RoleEdit"></pageTable>
  </div>
</template>
<script>
import PageTable from "@/components/PageTable";
import { store } from "@/vuex";
export default {
  data() {
    return {
      columnData: [
        { prop: "Name", label: "名称" },
        { prop: "Description", label: "描述" }
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
    },
    add() {
      this.$refs.pageTable.add();
    },
    update() {
      this.$refs.pageTable.update();
    }
  }
};
</script>