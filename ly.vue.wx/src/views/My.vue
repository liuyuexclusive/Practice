<template>
  <div>
    <x-button @click.native="logout">注销</x-button>
  </div>
</template>

<script>
import Vue from "vue";
import { XButton, ConfirmPlugin } from "vux";
Vue.use(ConfirmPlugin);

export default {
  data() {
    return {};
  },
  components: {
    XButton: XButton
  },
  methods: {
    logout() {
      let router = this.$router;
      this.$vux.confirm.show({
        // 组件除show外的属性
        onCancel() {},
        onConfirm() {
          localStorage.removeItem("token");
          router.push({ name: "Login" });
        }
      });
    }
  }
};
</script>

