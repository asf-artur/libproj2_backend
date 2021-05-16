using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebApplication2.Contracts;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // var selenium = new SeleniumGetSearchInfo("https://www.rsl.ru/");
            // var result = selenium.GetAllFirstPageJsons("мур");
            //
            // using (var sw = new StreamWriter("1.txt"))
            // {
            //     result.Jsons.ForEach(c => sw.WriteLine(c));
            //     Console.WriteLine("Hello World!");
            // }

            // var web = new Web();
            // web.Get();

            // var f = new Point("f");

            Db1.Go();


            var qq = 0;
            Console.WriteLine("finished");
            Console.Read();
        }

        public static async void R1()
        {
            Console.WriteLine("start R1111");
            await Task.Run(() => R());
            Console.WriteLine("end R111111");
        }

        public static void R()
        {
            Console.WriteLine("start R");
            Thread.Sleep(8000);
            Console.WriteLine("end R");
        }

        public async static void Fire()
        {
            Console.WriteLine($"started");
            var app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("firebase_key.json")
            });

            var clientToken = "e-sI-s0rQ0Swe13jL8ntTb:APA91bG7JuZ9w2dnUJhssWSi-D3ZHiCc-uOXG1PHJ0mzMcpMxZnn4DMKydEFgI_6-VwQ5GFIPEjyO2Z37MVpQzgw5amCCgI6e1w_H9i8Iw_GkOpJ17PeGdzPW4E_IYk4nVo5Cf796VD4";

            var message = new Message()
            {
                Token = clientToken,
                Notification =new Notification()
                {
                    Title = "C# notificiation",
                    Body = "Success!"
                }
            };

            Console.WriteLine($"wait for response");

            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            Console.WriteLine($"received: {response}");
        }

        public void go()
        {
            var allNames = new Dictionary<string, List<string>>();
            var books = new List<Book>();

            using (var sr = new StreamReader("1.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var t1 = sr.ReadLine();
                    var j1 = JsonConvert.DeserializeObject(t1) as JObject;

                    var bookConverterService = new BookConverterService();
                    var r = bookConverterService.GetBookFromJObject(j1);
                    books.Add(r);

                    j1.Properties()
                        .ToList()
                        .ForEach(c =>
                        {
                            if (!allNames.ContainsKey(c.Name) || allNames?[c.Name] == null)
                            {
                                allNames[c.Name] = new List<string>();
                            }
                            allNames[c.Name].Add(c.Value.ToString());
                        });
                    var q = 0;
                }
            }
        }
    }
}