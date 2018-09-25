<template>
  <el-form :model="formModel" :rules="rules" ref="formModel" label-width="100px">
    <el-form-item label="名称" prop="Name">
      <el-input v-model="formModel.Name"></el-input>
    </el-form-item>
    <el-form-item label="描述" prop="Description">
      <el-input type="textarea" :autosize="{ minRows: 4, maxRows: 6}" v-model="formModel.Description"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button @click="cancel('formModel')">取消</el-button>
      <el-button v-if="isShowSubmitWithoutClose" @click="submitWithoutClose('formModel')">继续</el-button>
      <el-button type="primary" @click="submit('formModel')">确定</el-button>
    </el-form-item>
  </el-form>
</template>
<script>
import { request } from "@/api";

export default {
  data() {
    return {
      isShowSubmitWithoutClose: false,
      formModel: {
        ID: null,
        Name: "",
        Description: ""
      },
      rules: {
        Name: [{ required: true, message: "请输入名称", trigger: "blur" }]
      }
    };
  },
  mounted() {
    this.formModel = this.$route.params;
    if (!this.$route.params.ID) {
      this.isShowSubmitWithoutClose = true;
    }
  },
  methods: {
    cancel(formName) {
      this.$router.push({ name: "Role" });
    },
    submitWithoutClose(formName) {
      this.submit(formName, false);
    },
    submit(formName, isClosed = true) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          request("Role/AddOrUpdate", "post", this.formModel).then(data => {
            if (data) {
              if (isClosed) {
                this.$router.push({ name: "Role" });
              }
              this.$refs[formName].resetFields();
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

