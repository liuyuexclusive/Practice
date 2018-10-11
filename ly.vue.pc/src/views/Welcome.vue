<template>
    <div>
         login time:{{time}}
    </div>
</template>

<script>
export default {
    data(){
        return{
            time:new Date().toLocaleString()
        }
    }
}
</script>
