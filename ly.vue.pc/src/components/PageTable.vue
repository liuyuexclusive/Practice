 <template>
  <div v-loading="loading">
    <el-table
      ref="commonPageTable"
      @row-dblclick="dblclick"
      :height="tableHight"
      :data="tableData"
      @current-change="currentChange"
      @selection-change="selectionChange"
      highlight-current-row
    >
      <el-table-column
        type="selection"
        width="50"
      ></el-table-column>
      <el-table-column
        :key="item.ID"
        v-for="item in columnData"
        :prop="item.prop"
        :label="item.label"
      ></el-table-column>
    </el-table>
    <el-pagination
      style="margin-top:10px"
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
      :current-page="pageData.CurrentPage"
      :page-sizes="pageSizes"
      :page-size="pageData.CurrentPageSize"
      :layout="pageData.layout"
      :total="pageData.Total"
    >
    </el-pagination>
  </div>

</template>

<script>
import { request } from "@/api";
import { store } from "@/vuex";
export default {
  data() {
    return {
      pageData: {
        layout:"total, sizes, prev, pager, next, jumper",
        CurrentPage: 1,
        CurrentPageSize: store.state.pageSize,
        Total: 0
      },
      loading: true,
      currentRow: null,
      selection: null,
      tableData: null,
      tableHight: window.innerHeight - 160 + "px"
    };
  },

  props: ["columnData", "getListUrl", "deleteUrl", "editRouteName","isPaging"],

  
  mounted() {
    this.refresh();
    window.onresize = () => {
      this.tableHight = window.innerHeight - 160 + "px";
    };
  },

  computed: {
    pageSizes() {
      return store.state.pageSizes;
    }
  },

  methods: {
    handleSizeChange(val) {
      this.pageData.CurrentPageSize = val;
      this.refresh();
    },
    handleCurrentChange(val) {
      this.pageData.CurrentPage = val;
      this.refresh();
    },
    currentChange(currentRow, oldCurrentRow) {
      this.currentRow = currentRow;
      this.$refs.commonPageTable.clearSelection();
      this.$refs.commonPageTable.toggleRowSelection(currentRow, true);
    },
    selectionChange(selection) {
      this.selection = selection;
    },
    refresh() {
      this.loading = true;
      if(this.isPaging!=true){
          this.pageData.layout="total";
          this.pageData.CurrentPage=1;
          this.pageData.CurrentPageSize=1000000;
      }
      request(this.getListUrl, "post", {CurrentPage:this.pageData.CurrentPage,CurrentPageSize:this.pageData.CurrentPageSize}).then(data => {
        if (data) {
          this.tableData = data.Data;
          this.pageData.Total = data.Total;
        }
        this.loading = false;
      });
    },
    remove() {
      if (!this.selection) {
        this.$alert("请选择行");
      } else {
        this.$confirm("删除数据将无法恢复，确认删除？").then(() => {
          request(this.deleteUrl, "delete", {
            IDs: this.selection.map(x => x.ID)
          }).then(data => {
            if (data) {
              this.refresh();
            }
          });
        });
      }
    },
    add() {
      this.$router.push({ name: this.editRouteName });
    },
    update() {
      if (!this.currentRow) {
        this.$alert("请选择行");
      } else {
        this.$router.push({
          name: this.editRouteName,
          params: this.currentRow
        });
      }
    },
    dblclick(row, event) {
      this.currentRow = row;
      this.update();
    }
  }
};
</script>