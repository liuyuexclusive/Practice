 <template>
  <div>
    <el-row type="flex" class="row-bg">
      <el-button @click="add">新增</el-button>
    </el-row>
  </div>
</template>
<script>
import PageTable from "@/components/PageTable";
export default {
  data() {
    return {
     
    };
  },
  components: {
    pageTable: PageTable
  },
  methods: {
    add() {
      this.$refs.pageTable.add();
    }
  }
};
</script>