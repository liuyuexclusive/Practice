<template>
  <div style="padding:20px">
    <x-input
      name="email"
      v-model="data.Email"
      placeholder="请输入邮箱地址"
      is-type="email"
    ></x-input>
    <x-input
      name="password"
      v-model="data.Password"
      placeholder="请输入密码"
      type="password"
    ></x-input>
    <x-button @click.native="login">登录</x-button>
    <div style="text-align:center;margin-top:20px;">还没有账号？请点击<router-link to="Register">注册</router-link>
    </div>
  </div>
</template>
<script>
import { XButton, XInput } from "vux";
import { request } from "@/api";

export default {
  data() {
    return {
      data: {
        Email: "453017973@qq.com",
        Password: "1234"
      }
    };
  },
  methods: {
    login() {
      request("User/Login", "put", this.data).then(data => {
        if (data) {
          localStorage.token = data.Data.Token;
          localStorage.userName = data.Data.UserName;
          this.$router.push({ name: "My" });
        }
      });
    }
  },
  components: {
    XButton,
    XInput
  }
};
</script>

