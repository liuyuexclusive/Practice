<template>
  <div style="padding:20px">
    <form method="post"> 
      <x-input name="Email" placeholder="请输入邮箱地址" type="email" is-type="email" required="true"></x-input>
      <x-input name="Password" placeholder="请输入密码" type="password" required="true"></x-input>
      <x-input name="ConfirmPassword" placeholder="确认密码" type="password" required="true"></x-input>
      <x-input name="Name" placeholder="用户名"></x-input>
      <x-input name="Mobile" placeholder="电话" type="tel"></x-input>
      <x-input name="ValidateCode" placeholder="验证码" type="number"></x-input>
      <x-button>注册</x-button>
    </form>
  </div>
</template>
<script>
import { XButton,XInput  } from "vux";

export default {
  components: {
    XButton,XInput
  }
};
</script>