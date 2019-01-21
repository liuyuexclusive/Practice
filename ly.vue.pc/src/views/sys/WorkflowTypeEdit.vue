<template>
  <div>
    <div style="height: 300px;">
      <el-steps simple>
      <el-step v-for="item in arr" :title="item.text" :id="item.id" draggable="true" @dragstart="dragstart($event)" @drop="drop($event)" @dragover="dragover($event)">
        <div></div>
      </el-step>
      </el-steps>
    </div>
    <ul>
      <li v-for="item in arr" :id="item.id" draggable="true" @dragstart="dragstart($event)" @drop="drop($event)" @dragover="dragover($event)">{{item}}</li>
    </ul>
    <!-- <div
      id="drag1"
      draggable="true"
      @dragstart="drag($event)"
    >主要按钮</div>
    <div
      id="div1"
      @drop="drop($event)"
    ></div> -->
  </div>
</template>
<style>
#div1 {
  width: 350px;
  height: 70px;
  padding: 10px;
  border: 1px solid #aaaaaa;
}
</style>

<script>
import { request } from "@/api";

export default {
  data() {
    return {
      arr:[
        {id:1,text:"111111111111"},
        {id:2,text:"222222222222"},
        {id:3,text:"333333333333"}
      ]
    };
  },
  methods: {
    dragover(ev) {
      ev.preventDefault();
    },
    dragstart(ev) {
      ev.dataTransfer.setData("Text", ev.target.id);
    },
    drop(ev) {
      var arr = this.arr;
      let index1 = arr.indexOf(arr.find(x=>x.id==ev.dataTransfer.getData("Text")));
      let index2 = arr.indexOf(arr.find(x=>x.id==ev.target.id));
      arr[index1]=(arr.splice(index2,1,arr[index1]))[0];
      //et temp = arr[index2];
      // arr.splice(x - 1, 1, ...arr.splice(y - 1, 1, arr[x - 1]))
      // arr[index2] = arr[index1]
      // arr[index1] = temp
      // console.log(this.arr)
      // console.log(id2)
      // ev.target.appendChild(document.getElementById(data));
    }
  }
};
</script>

