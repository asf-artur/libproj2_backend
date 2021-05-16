<template>
  <div class="login_form">
    <input v-model="login" type="text" name="login" placeholder="логин"/>
    <input v-model="password" type="password" name="passwrd" placeholder="пароль"/>
    <button v-on:click="okClick">войти</button>
  </div>
</template>

<script>
import DataCollector from "@/func/DataCollector";
import User from "@/model/User";

export default {
  name: "Login",
  data(){
    return{
      login:"",
      password:"",
    }
  },
  methods:{
    okClick(){
      if(this.login === "" && this.password === ""){
      }
      else{
        let dataCollector = new DataCollector()
        const observer = {
          next: json => {
            console.log('Observer got a next value: ' + json)
            let user = new User(json)
            this.$emit('userSet', user)
          },
          error: err => console.error('Observer got an error: ' + err),
          complete: () => console.log('Observer got a complete notification'),
        };

        let response =  dataCollector.login(this.login, this.password)
        response.subscribe(observer)
      }
    }
  }
}
</script>

<style scoped>
  .login_form{
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
  .login_form *{
    margin-bottom: 10px;
  }
</style>