using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Web
    {
        public void Get()
        {
            var parameters = new List<(string, string)>
            {
                ("q", "Flowers")
            };

            var id = "wfFFAQAAIAAJ";
            var path = "https://www.googleapis.com/books/v1/volumes/";
            path += id;
            var req = WebRequest.Create(path);
            req.Method = WebRequestMethods.Http.Get;

            var resp = req.GetResponse();
            using (var sr = new StreamReader(resp.GetResponseStream()))
            {
                var i = 0;
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadToEnd();
                    line.Split("\n").ToList()
                        .ForEach(c => Console.WriteLine($"{i++}){c}"));

                    var json = JsonConvert.DeserializeObject<BookGoogle>(line);
                    var q = 0;
                }
            }
        }

        private string GetParametersString(List<(string, string)> list)
        {
            var result = list
                .Select(c =>
                {
                    var end = c == list.Last() ? "" : "&";
                    return $"{c.Item1}={c.Item2}{end}";
                })
                .Aggregate((a, b) => a + b);

            return result;
        }
    }
}