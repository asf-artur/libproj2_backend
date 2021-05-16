<template>
  <div class="profile_info">
    <img src="../assets/user.svg" alt="">
    <div class="info">
      <div>{{user.name}}</div>
      <div>{{user.barcode}}</div>
    </div>
  </div>
</template>

<script>
import DataCollector from "@/func/DataCollector";
import User from "@/model/User";

export default {
  name: "Profile",
  data(){
    return{
      user: User,
    }
  },
  created() {
    this.user = this.$store.state.user
  },
  methods:{
  }
}
</script>

<style scoped>
  .profile_info{
    margin-top: 5vh;
    height: 100px;
    display: flex;
    margin-left: 5vw;
  }
  .profile_info > img{
    height: 100%;
  }
  .info{
    height: 100%;
    margin-left: 10px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
  }
  .info *{
    margin-top: 10px;
  }
</style>