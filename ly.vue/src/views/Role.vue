 <template>
    <div>
        <el-row type="flex" class="row-bg">
            <el-tooltip class="item" effect="dark" content="新增" placement="bottom-start">
                <el-button @click="add" icon="el-icon-plus" circle></el-button>
            </el-tooltip>
            <el-tooltip class="item" effect="dark" content="编辑" placement="bottom-start">
                <el-button icon="el-icon-edit" circle></el-button>
            </el-tooltip>
            <el-tooltip class="item" effect="dark" content="删除" placement="bottom-start">
                <el-button icon="el-icon-delete" circle></el-button>
            </el-tooltip>
            <el-tooltip class="item" effect="dark" content="刷新" placement="bottom-start">
                <el-button @click="refresh" icon="el-icon-refresh" circle></el-button>
            </el-tooltip>
        </el-row>
        <el-table :data="tableData" @current-change="currentChange" highlight-current-row height="350px" style="margin-bottom:20px">
            <el-table-column prop="Name" label="名称" width="140">
            </el-table-column>
            <el-table-column prop="Description" label="描述" width="120">
            </el-table-column>
        </el-table>
    </div>

</template>

<script>
import { getRoles } from "@/api";
export default {
  data() {
    return {
      currentRow: null,
      tableData: null
    };
  },

  mounted() {
    this.refresh();
  },

  methods: {
    refresh() {
      getRoles().then(data => {
        this.tableData = data.Data;
      });
    },
    add() {
      this.$router.push({ path: "/RoleEdit" });
    },
    currentChange(currentRow, oldCurrentRow) {
      currentRow = currentRow;
    }
  }
};
</script>