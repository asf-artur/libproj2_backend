import { createApp } from 'vue'
import {createRouter, createWebHistory} from 'vue-router'
import { createStore } from 'vuex'
import createPersistedState from "vuex-persistedstate";
import App from './App.vue'
import DataCollector from "@/func/DataCollector";
import HelloWorld from "@/components/HelloWorld";
import NotFound from "@/components/NotFound";
import Profile from "@/components/Profile";
import Login from "@/components/Login";
import BookEvents from "@/components/BookEvents";

const Foo = HelloWorld
const Bar = { template: '<div>bar</div>' }
const About = { template: '<div>About Page</div>', name: 'About' }
const NotFound1 = NotFound
const routes = [
    {path: '/foo', component: BookEvents, name: 'BookEvents'},
    { path: '/about', component: NotFound },
    { path: '/login', component: Login },
    {path: '/profile', component: Profile, name: 'Profile', props:true},
    // will match everything and put it under `$route.params.pathMatch`
    { path: '/:pathMatch(.*)*', name: 'NotFound', component: NotFound },
]
const router = createRouter({
    history: createWebHistory(),
    routes
})
const store = createStore({
    plugins: [createPersistedState()],
    state(){
        return {
            selectedButton: -1,
            logined: false,
            user: null,
        }
    },
    mutations:{
        setSelectedButton(state, num){
            state.selectedButton = num
        },
        setLogined(state, logined){
            state.logined = logined
        },
        setUser(state, user){
            state.user = user
        }
    }
})

createApp(App)
    .use(router)
    .use(store)
    .mount('#app')
