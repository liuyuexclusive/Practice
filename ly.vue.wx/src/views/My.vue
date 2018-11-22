<template>
  <div style="padding:20px">
    <x-input title="邮箱" name="email" placeholder="请输入邮箱地址" is-type="email"></x-input>
    <x-input title="密码" name="password" placeholder="请输入密码" type="password"></x-input>
    <flexbox>
      <flexbox-item>
        <x-button type="primary">登录</x-button>
      </flexbox-item>
      <flexbox-item>
        <x-button type="primary">注册</x-button>
      </flexbox-item>
    </flexbox>
  </div>
</template>
<script>
import { XButton,Flexbox, FlexboxItem,XInput  } from "vux";

export default {
  components: {
    XButton,Flexbox, FlexboxItem,XInput
  }
};
</script>

