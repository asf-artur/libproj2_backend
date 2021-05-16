<template>
  <div class="grid-container">
    <div class="item left">
      <button v-bind:class="{selected: selectedButton===0}" v-on:click="go1" class="btn1">Профиль</button>
      <button v-bind:class="{selected: selectedButton===1}" v-on:click="go2" class="btn2">События с книгами</button>
    </div>
    <div class="item center">
      <div class="content">
        <router-view
            v-on:userSet="getUser"/>
      </div>
    </div>
    <div class="item right"></div>
  </div>
</template>

<script>
import DataCollector from "@/func/DataCollector";
import User from "@/model/User";

export default {
  name: "Main",
  data(){
    return{
      name: "ФИОООО",
      barcode: "32432423545",
      user: null,
    }
  },
  computed:{
    selectedButton() {
      return this.$store.state.selectedButton
    }
  },
  created(){
    let currentPath = this.$router.options.history.location
    if(currentPath === '/foo'){
      this.$store.commit('setSelectedButton', 1)
    }
    else if(currentPath === '/profile' || currentPath === '/login'){
      this.$store.commit('setSelectedButton', 0)
    }
    else{
      this.$store.commit('setSelectedButton', -1)
    }
    let q = 0
  },
  methods:{
    getUser(user){
      this.user = user
      this.$store.commit('setLogined', true)
      this.$store.commit('setUser', user)
      this.$router.push({name:"Profile"})
    },
    go1(){
      // current
      // let c = this.$router.currentRoute.value.name
      this.$store.commit('setSelectedButton', 0)
      if(this.user == null){
        let dataCollector = new DataCollector()
        const observer = {
          next: json => {
            console.log('Observer got a next value: ' + json)
            if(json.error === true){
              this.$router.push('/login')
            }
            else{
              let user = new User(json)
              this.user = user
              this.$router.push({name:"Profile"})
            }
          },
          error: err => {
            console.error('Observer got an error: ' + err)
            this.$router.push('/login')
          },
          complete: () => console.log('Observer got a complete notification'),
        };

        let response = dataCollector.checkLogin()
        response.subscribe(observer)
        let q = 0
      }
      else{
        this.$router.push({name:"Profile"})
      }
    },
    go2(){
      this.$router.push('/foo')
      this.$store.commit('setSelectedButton', 1)
      // this.selectedButton = 1
    }
  }
}
</script>

<style scoped>
  .grid-container{
    background-color: #e9fafa;
    display: grid;
    grid-template-columns: 1fr 60vw 1fr;
    min-height: 100vh;
  }
  .left{
    background-color: #e0f6ed;
    /*background-color: #bff6dd;*/
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  .btn1{
    margin-top: 5vw;
  }
  .center{
    /*background-color: #e9fafa;*/
    display: flex;
    flex-direction: column;
  }
  .right{
    /*background-color: #f59999;*/
  }
  button{
    margin-bottom: 20px;
    width: 80%;
    max-width: 150px;
    padding: 8px 0;
    background-color: #81E4E5;
    border-radius: 5px;
    border: none;
    text-decoration: none; /* убирать подчёркивание у ссылок */
    user-select: none; /* убирать выделение текста */
    outline: none;
  }
  button:hover{
    background-color: #77bfe7;
    cursor: pointer;
  }
  button:active{
    background-color: #617cf6;
  }
  button[class*="selected"]{
    background-color: #B64A5F;
  }
  button[class*="selected"]:hover{
    background-color: #c67f8e;
    cursor: pointer;
  }
  button[class*="selected"]:active{
    background-color: #d02142;
  }
  .content{
    margin-top: 5vw;
    /*border: 1px solid #d02142;*/
    /*margin-left: 5vw;*/
    min-width: 100%;
    display: flex;
    justify-content: center;
  }
</style>