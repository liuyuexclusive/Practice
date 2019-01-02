<template>
  <div>
    <h1>{{socketState}}</h1>

    <el-row><textarea v-model="content" type="textarea" id="sendMessage" rows="10" cols="200" /></el-row>
    <el-row><button @click="send">Send</button></el-row>

    <h2>Communication Log</h2>
    <ul>
      <li v-for="log in logs">

        {{log}}
      </li>
    </ul>
  </div>
</template>

<script>
var socket;
import { sysSocket } from '@/socket'
console.log(window.location.host)
export default {
  data() {
    return {
      logs: [],
      content: null,
      socketState: null
    };
  },
  created() {
    if (!socket) {
      socket = sysSocket("Test");
      socket.onopen = event => {
        this.logs.push("open");
        console.log(socket)
      };
      socket.onclose = event => {
        this.logs.push("close");
      };
      socket.onmessage = event => {
        console.log(event)
        this.logs.push(event.data.toString());
      };
    }
  },
  destroyed(){
    if(socket){
      socket.close();
      socket = null;
    }
  },
  watch: {},
  methods: {
    close() {
      if (!socket || socket.readyState !== WebSocket.OPEN) {
        alert("socket not connected");
      }
      socket.close(1000, "Closing from client");
    },
    send() {
      socket.send(this.content);
    }
  }
};
</script>

<style>
table {
  border: 0;
}

.commslog-data {
  font-family: Consolas, Courier New, Courier, monospace;
}

.commslog-server {
  background-color: red;
  color: white;
}

.commslog-client {
  background-color: green;
  color: white;
}
</style>
