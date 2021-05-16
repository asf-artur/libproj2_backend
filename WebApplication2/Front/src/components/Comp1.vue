<template>
  <div>{{text}}</div>
  <button v-on:click="go()">Кнопка</button>
  <br>
  <br>
  <br>
  <router-link to="/foo">Ссылка</router-link>
  <router-link to="/about">Go to About</router-link>
  <br>
  <br>
  <router-view></router-view>
</template>

<script>
import { Observable, of } from 'rxjs';
import { fromFetch } from "rxjs/fetch";
import { switchMap, catchError } from 'rxjs/operators';
import DataCollector from "@/func/DataCollector";
export default {
  name: "Comp1",
  data(){
    return{
      text : '----'
    }
  },
  methods:{
    go : async function () {
      let d = new DataCollector()

      const observer = {
        next: x => {
          console.log('Observer got a next value: ' + x)
          this.text += x
        },
        error: err => console.error('Observer got an error: ' + err),
        complete: () => console.log('Observer got a complete notification'),
      };

      let result = d.index()
      result.subscribe(observer)

      let a = 0
    }
  }
}
</script>

<style scoped>

</style>