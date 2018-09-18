<template>
    <el-form :model="formModel" :rules="rules" ref="formModel" label-width="100px">
        <el-form-item label="名称" prop="Name">
            <el-input v-model="formModel.Name"></el-input>
        </el-form-item>
        <el-form-item label="描述" prop="Description">
            <el-input type="textarea" :autosize="{ minRows: 4, maxRows: 6}" v-model="formModel.Description"></el-input>
        </el-form-item>
        <el-form-item>
            <el-button @click="cancelForm('formModel')">取消</el-button>
            <el-button @click="resetForm('formModel')">重置</el-button>
            <el-button type="primary" @click="submitForm('formModel')">确定</el-button>
        </el-form-item>
    </el-form>
</template>
<script>
import {addRole} from "@/api"

export default {
  data() {
    return {
      formModel: {
        Name: "",
        Description: ""
      },
      rules: {
          Name: [{required: true, message: "请输入名称", trigger: "blur"}]
      }
    };
  },
  methods: {
    cancelForm(formName) {
      this.$router.push({ path: "/Role" });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          addRole(this.formModel).then(data => {
            if (data.Success === true) {
              this.$alert(data.Message).then(() => {
                this.$router.push({ path: "/Role" });
              });
            } else {
              this.$alert(data.Message, "", { type: "warning" });
            }
          });
        } else {
          return false;
        }
      });
    }
  }
};
</script>

