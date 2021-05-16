 <template>
   <Main/>
</template>

<script>
import HelloWorld from './components/HelloWorld.vue'
import Comp1 from "@/components/Comp1";
import Main from "@/components/Main";

export default {
  name: 'App',
  components: {
    Main,
    Comp1
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>
