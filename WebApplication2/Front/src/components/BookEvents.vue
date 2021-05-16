<template>
  <div>
    <button @click="refresh1">Обновить</button>
    <div class="list">
      <div class="element" v-for="bookEvent in bookEventList">{{bookEvent}}</div>
    </div>
  </div>

</template>

<script>
import DataCollector from "@/func/DataCollector";
import User from "@/model/User";

export default {
  name: "BookEvents",
  data(){
    return{
      start: true,
      bookEventList: null,
    }
  },
  // mounted() {
  //   let func = this.refresh1
  //   this.$refs.mybtn1.onpointerdown = function () {
  //     func()
  //   }
  // },
  methods:{
    refresh1(){
      console.log("dddddddddddddddddddd")
      let dataCollector = new DataCollector()
      const observer = {
        next: json => {
          console.log('Observer got a next value: ' + json)
          this.bookEventList = json
        },
        error: err => {
          console.error('Observer got an error: ' + err)
        },
        complete: () => {
          console.log('Observer got a complete notification')
        }
      };
      let response = dataCollector.GetAllBookEvents()
      response.subscribe(observer)
    }
  }
}
</script>

<style scoped>
  .list{
    /*background-color: #efa9b7;*/
    /*border: 1px #ff0000 solid;*/
    min-height: 40px;
    min-width: 100%;
    border-radius: 5px;
    display: flex;
    flex-direction: column;
    max-height: 100vh;
    overflow: auto;
  }
  .element{
    border: 1px #ff0000 solid;
    padding: 10px 0;
  }
</style>