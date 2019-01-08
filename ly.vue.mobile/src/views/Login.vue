<template>
  <div>
    <mt-field
      placeholder="请输入邮箱"
      type="email"
      v-model="data.Email"
    ></mt-field>
    <mt-field
      placeholder="请输入密码"
      type="password"
      v-model="data.Password"
    ></mt-field>
    <mt-button size="large" type="primary" @click.native="handleClick">登录</mt-button>
  </div>
</template>
<script>
export default { 
  data() {
    return {
      data: {
        Email: "",
        Password: ""
      }
    };
  }
};
</script>
