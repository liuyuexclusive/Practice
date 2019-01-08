<template>
  <div>
    <mt-header fixed :title="selected">
      <router-link
        to="/"
        slot="left"
      >
        <mt-button icon="back">返回</mt-button>
      </router-link>
      <mt-button
        icon="more"
        slot="right"
      ></mt-button>
    </mt-header>
    <mt-tab-container
      class="page-tabbar-container"
      v-model="selected"
    >
      <mt-tab-container-item id="Product">
        <Product></Product>
      </mt-tab-container-item>
      <mt-tab-container-item id="Order">
        <Order></Order>
      </mt-tab-container-item>
      <mt-tab-container-item id="My">
        <My></My>
      </mt-tab-container-item>
    </mt-tab-container>
    <mt-tabbar
      v-model="selected"
      fixed
    >
      <mt-tab-item id="Product">
        <img
          slot="icon"
          src="../assets/100x100.png"
        >
        产品
      </mt-tab-item>
      <mt-tab-item id="Order">
        <img
          slot="icon"
          src="../assets/100x100.png"
        >
        订单
      </mt-tab-item>
      <mt-tab-item id="My">
        <img
          slot="icon"
          src="../assets/100x100.png"
        >
        我的
      </mt-tab-item>
    </mt-tabbar>
  </div>
</template>

<script>
import Product from "@/views/Product.vue";
import Order from "@/views/Order.vue";
import My from "@/views/My.vue";
export default {
  name: "page-tabbar",
  data() {
    return {
      selected: "Product"
    };
  },
  components: {
    Product: Product,
    Order: Order,
    My: My
  }
};
</script>