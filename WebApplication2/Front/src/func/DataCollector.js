import { Observable, of, Observer  } from 'rxjs';
import { fromFetch } from "rxjs/fetch";
import { switchMap, catchError } from 'rxjs/operators';
import User from "@/model/User";

const LOCAL_URL = '/api/'
// if(process.env)
// const LOCAL_URL = 'http://localhost:5000/api/'
// const LOCAL_URL = 'http://192.168.31.21:44331/api/'
// const LOCAL_URL = 'http://152.70.52.35:3244/api/'

export default class DataCollector {
   constructor() {
   }

   checkLogin(){
       let api = 'user/checkLogin'
       let address = LOCAL_URL + api
       let response = fromFetch(address, {
           method: 'post',
           // credentials: 'include'
       }).pipe(
           switchMap(response =>{
               if(response.ok){
                   let result = response.json()
                   return result
               }
               else {
                   return of({ error: true, message: `Error ${response.status}` });
               }
           })
       )

       return response
   }

   login(login, password){
       let api = 'user/Login'
       let address = LOCAL_URL + api + `?Login=${login}&Password=${password}`
       let response = fromFetch(address, {
           method: 'post',
           // credentials: 'same-origin'
       }).pipe(
           switchMap(response =>{
               if(response.ok){
                   let result = response.json()
                   return result
               }
               else {
                   // Server is returning a status requiring the client to try something else.
                   return of({ error: true, message: `Error ${response.status}` });
               }
           })
       )

       return response
   }

    GetAllBookEvents(){
        let api = 'service/GetAllBookEvents'
        let address = LOCAL_URL + api
        let response = fromFetch(address, {
            method: 'get',
        }).pipe(
            switchMap(response =>{
                if(response.ok){
                    let result = response.json()
                    // let result = response.text()
                    return result
                }
                else {
                    return of({ error: true, message: `Error ${response.status}` });
                }
            })
        )

        return response
    }

   index(){
      let api = 'home/index'
      let address = LOCAL_URL + api
      let j = fromFetch(address)
          .pipe(
              switchMap(response =>{
                 if(response.ok){
                    let result = response.json()
                    return result
                 }
                 else {
                    // Server is returning a status requiring the client to try something else.
                    return of({ error: true, message: `Error ${response.status}` });
                 }
              })
          )
      const observer = {
         next: x => console.log('Observer got a next value: ' + x),
         error: err => console.error('Observer got an error: ' + err),
         complete: () => console.log('Observer got a complete notification'),
      };

      return j
   }
}