<template>
  <div
    class="workflow"
    v-loading="loading"
  >
    <el-steps>
      <el-step
        v-for="(model,index) in stepData"
        :key="index"
        :title="model.Name"
        :description="model.Description"
      ></el-step>
    </el-steps>
    <el-form
      size="mini"
      :model="workflowForm"
      :rules="workflowRules"
      ref="workflowForm"
    >
      <el-form-item
        prop="Name"
        label="流程名称"
      >
        <el-input
          style="width:200px;"
          v-model="workflowForm.Name"
          placeholder="请输入节点名称"
        >
        </el-input>
      </el-form-item>
    </el-form>
    <el-form
      :inline="true"
      size="mini"
      :model="nodeForm"
      :rules="nodeRules"
      ref="nodeForm"
    >
      <div
        v-for="(model,index) in nodes"
        :id="model.ID"
        class="workflowNode"
        draggable="true"
        @dragstart="dragstart($event)"
        @dragover="dragover($event)"
        @dragenter="dragenter($event)"
      >
        <el-form-item
          :label="'节点'+(index+1)"
          :prop="'Name'+index"
          :required="true"
        >
          <el-input
            style="width:200px;"
            v-model="model.Name"
            placeholder="请输入节点名称"
          >
          </el-input>
        </el-form-item>
        <el-form-item
          :prop="'Auditors'+index"
          :required="true"
        >
          <el-select
            style="width:350px"
            v-model="model.Auditors"
            multiple
            placeholder="请选择审核人"
            filterable
          >
            <el-option
              v-for="item in auditors"
              :key="item.UserID"
              :label="item.UserName"
              :value="item.UserID"
            >
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button
            type="danger"
            icon="el-icon-delete"
            circle
            @click="remove(index)"
          ></el-button>
        </el-form-item>
      </div>
    </el-form>
    <el-button @click="add()">新增节点</el-button>
    <el-button @click="save()">保存</el-button>
  </div>
</template>
<style>
.workflow {
  overflow-y: auto;
  height: 100%;
}
.workflowNode {
  margin-top: 10px;
  margin-bottom: 10px;
  border: solid 1px lightgray;
  padding: 10px;
}
</style>

<script>
import { request } from "@/api";
import { v1 } from "uuid";
export default {
  data() {
    return {
      selectedID: 1,
      loading: false,
      nodes: [],
      auditors: [
        { UserID: 1, UserName: "张三" },
        { UserID: 2, UserName: "李四" },
        { UserID: 3, UserName: "王五" },
        { UserID: 4, UserName: "赵六" }
      ],
      workflowForm: {
        Name: "",
        ID: null
      },
      workflowRules: {
        Name: [{ required: true, message: "请输入节点名称", trigger: "blur" }]
      }
    };
  },
  mounted() {
    var param = this.$route.params;
    this.workflowForm.ID = param.ID;
    this.workflowForm.Name = param.Name;
    request("WorkflowType/GetNodes", "post", { ID: param.ID }).then(data => {
      if (data) {
        this.nodes = data.Data;
        if (!this.nodes) {
          this.nodes = [];
        }
      }
    });
  },
  computed: {
    //for validate
    nodeForm() {
      let result = {};
      let nodes = this.nodes;
      for (let index in nodes) {
        result["Name" + index] = nodes[index].Name;
        result["Auditors" + index] = nodes[index].Auditors;
      }
      return result;
    },
    //for validate
    nodeRules() {
      let result = {};
      let nodes = this.nodes;
      for (let index in nodes) {
        result["Name" + index] = [
          { required: true, message: "请输入节点名称", trigger: "blue" }
        ];
        result["Auditors" + index] = [
          { required: true, message: "请选择审核人", trigger: "change" }
        ];
      }
      return result;
    },
    stepData() {
      this.nodes.forEach(item => {
        item.Description =
          "审核人：" +
          this.auditors
            .filter(x => item.Auditors.some(y => y == x.UserID))
            .map(x => x.UserName)
            .join(",");
      });
      return this.nodes;
    }
  },
  methods: {
    save() {
      this.$refs["workflowForm"].validate(valid => {
        if (valid) {
          this.$refs["nodeForm"].validate(valid => {
            if (valid) {
              this.loading = true;
              let param = {};
              param.ID = this.workflowForm.ID;
              param.Name = this.workflowForm.Name;
              param.Nodes = this.nodes;
              request("WorkflowType/AddOrUpdate", "post", param).then(data => {
                if (data) {
                  this.$router.push({ name: "WorkflowType" });
                }
                this.loading = false;
              });
            } else {
              return false;
            }
          });
        } else {
          return false;
        }
      });
    },
    add() {
      let key = v1();
      this.nodes.push({ ID: key, Name: "", Auditors: [] });
    },
    remove(index) {
      this.nodes.splice(index, 1);
    },
    dragover(ev) {
      ev.preventDefault();
    },
    dragstart(ev) {
      this.selectedID = ev.target.id;
    },
    dragenter(ev) {
      if (ev.target.tagName != "DIV") {
        return;
      }
      let nodes = this.nodes;
      let index1 = nodes.indexOf(nodes.find(x => x.ID == this.selectedID));
      let index2 = nodes.indexOf(nodes.find(x => x.ID == ev.target.id));
      nodes[index1] = nodes.splice(index2, 1, nodes[index1])[0];
    }
  }
};
</script>

