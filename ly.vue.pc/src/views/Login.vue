<template>
  <div>
    <el-row type="flex" class="row-bg" style="margin-top: 15%;" justify="center">
      <el-col :span="6">
        <el-card class="box-card">
          <el-form :model="formModel" :rules="rules" style="margin:0px auto;" ref="formModel">
            <el-form-item prop="Email">
              <el-input v-model="formModel.Email" placeholder="邮箱"></el-input>
            </el-form-item>
            <el-form-item prop="Password">
              <el-input type="password" v-model="formModel.Password" placeholder="密码" @keyup.enter.native="submitForm('formModel')"></el-input>
            </el-form-item>
            <el-form-item>
              <el-button @click="register">注册</el-button>
              <el-button type="primary" style="float:right" @click="submitForm('formModel')">登录</el-button>
            </el-form-item>
          </el-form>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { request } from "@/api";
export default {
  data() {
    let validatEmail = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入邮箱"));
      } else if (
        !/^[A-Za-z\d]+([-_.][A-Za-z\d]+)*@([A-Za-z\d]+[-.])+[A-Za-z\d]{2,4}$/.test(
          value
        )
      ) {
        callback(new Error("请输入有效邮箱!"));
      } else {
        callback();
      }
    };
    return {
      formModel: {
        Email: "",
        Password: ""
      },
      rules: {
        Email: [{ required: true, validator: validatEmail, trigger: "blur" }],
        Password: [{ required: true, message: "请输入密码", trigger: "blur" }]
      }
    };
  },

  methods: {
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          request("User/Login", "put", this.formModel)
            .then(data => {
              if (data) {
                localStorage.token = data.Data.Token;
                localStorage.userName = data.Data.UserName;
                this.$router.push({ name: "Welcome" });
              }
            })
        } else {
          return false;
        }
      });
    },
    register: function() {
      this.$router.push({
        name: "Register"
      });
    }
  }
};
</script>
