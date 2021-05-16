using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleApp1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Books;
using BookConverterService = WebApplication2.Services.BookConverterService;

namespace WebApplication2.Repositories
{
    public class BookSearchResultRepository
    {
        public List<BookInfo> GetAll()
        {
            var bookConverterService = new BookConverterService();

            var result = new List<BookInfo>();
            var id = 0;
            using (var sr = new StreamReader("1.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var jObject = JsonConvert.DeserializeObject(line) as JObject;
                    var book = bookConverterService.GetBookFromJObject1(jObject, id);

                    result.Add(book);
                    id++;
                }
            }

            return result;
        }
    }
}