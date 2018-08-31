 <template>
    <div>
        <el-row type="flex" class="row-bg">
            <el-tooltip class="item" effect="dark" :content="add" placement="bottom-start">
                <el-button icon="el-icon-plus" circle></el-button>
            </el-tooltip>
            <el-tooltip class="item" effect="dark" :content="edit" placement="bottom-start">
                <el-button icon="el-icon-edit" circle></el-button>
            </el-tooltip>
            <el-tooltip class="item" effect="dark" :content="remove" placement="bottom-start">
                <el-button icon="el-icon-delete" circle></el-button>
            </el-tooltip>
            <el-tooltip class="item" effect="dark" :content="refresh" placement="bottom-start">
                <el-button icon="el-icon-refresh" circle></el-button>
            </el-tooltip>
        </el-row>
        <el-table :data="tableData" height="350px" style="margin-bottom:20px">
            <el-table-column prop="date" label="日期" width="140">
            </el-table-column>
            <el-table-column prop="name" label="姓名" width="120">
            </el-table-column>
            <el-table-column prop="address" label="地址">
            </el-table-column>
        </el-table>
        <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page="1" :page-sizes="[30, 60, 90]" :page-size="30" layout="total, sizes, prev, pager, next, jumper" :total="400">
        </el-pagination>
    </div>

</template>

<script>
export default {
  data() {
    const item = {
      date: "2016-05-02",
      name: "王小虎",
      address: "上海市普陀区金沙江路 1518 弄"
    };
    return {
      tableData: Array(20).fill(item),
      add: this.$t("message.add"),
      edit: this.$t("message.edit"),
      remove: this.$t("message.remove"),
      refresh: this.$t("message.refresh")
    };
  }
};
</script>