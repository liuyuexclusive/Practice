<template>
  <el-form :model="formModel" :rules="rules" ref="formModel" label-width="100px">
    <el-form-item label="邮箱" prop="Email">
      <el-input v-model="formModel.Email"></el-input>
    </el-form-item>
    <el-form-item label="密码" prop="Password">
      <el-input type="password" v-model="formModel.Password"></el-input>
    </el-form-item>
    <el-form-item label="确认密码" prop="confirmPassword">
      <el-input type="password" v-model="formModel.confirmPassword"></el-input>
    </el-form-item>
    <el-form-item label="用户名">
      <el-input v-model="formModel.Name"></el-input>
    </el-form-item>
    <el-form-item label="电话" prop="Mobile">
      <el-input v-model="formModel.Mobile"></el-input>
    </el-form-item>
    <el-form-item style="width:50%" label="验证码" prop="ValidateCode">
      <el-input v-model="formModel.ValidateCode">
        <el-button :disabled="disabledGetValidateCode" slot="append" @click="getValidateCode">{{validateText}}</el-button>
      </el-input>
    </el-form-item>
    </el-input>
    <el-form-item>
      <el-button @click="resetForm('formModel')">重置</el-button>
      <el-button type="primary" @click="submitForm('formModel')">注册</el-button>
    </el-form-item>
  </el-form>
</template>


<script>
import { register,getValidateCode } from "@/api";

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

    let validateMobile = (rule, value, callback) => {
      if (
        !/(^$)|(^1(3[0-9]|4[579]|5[0-35-9]|7[0-9]|8[0-9])\d{8}$)/.test(
          value
        )
      ) {
        callback(new Error("请输入有效电话!"));
      } else {
        callback();
      }
    };

    let validateconfirmPassword = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请再次输入密码"));
      } else if (value !== this.formModel.Password) {
        callback(new Error("两次输入密码不一致!"));
      } else {
        callback();
      }
    };

    return {
      validateText: "获取",
      disabledGetValidateCode:false,
      formModel: {
        Email: "yu-liu@qulv.com",
        Password: "",
        confirmPassword: "",
        Name: "",
        Mobile: "",
        ValidateCode: ""
      },
      rules: {
        Email: [{ required: true, validator: validatEmail, trigger: "blur" }],        
        Password: [{ required: true, message: "请输入密码", trigger: "blur" }],
        confirmPassword: [
          {
            required: true,
            validator: validateconfirmPassword,
            trigger: "blur"
          }
        ],
        Mobile:[{ validator:validateMobile, trigger: "blur" }],
        ValidateCode: [
          { required: true, message: "请输入验证码", trigger: "blur" }
        ]
      }
    };
  },
  methods: {
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          register(this.formModel).then(data => {
            if (data.Success === true) {
              this.$alert(data.Message).then(() => {
                this.$router.push({ path: "/" });
              });
            } else {
              this.$alert(data.Message, "", { type: "warning" });
            }
          });
        } else {
          return false;
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },
    getValidateCode() {
      getValidateCode({email:this.formModel.Email}).then(data=>{
         console.log(data);
      })
      this.disabledGetValidateCode =true;
      var i = 60;
      this.validateText = i;      
      var si = setInterval(()=>{
        i--;
        this.validateText = i;
        if (i==0) {
          clearInterval(si);
          this.disabledGetValidateCode = false;
          this.validateText = "获取";
        }
      },1000);
    }
  }
};
</script>

