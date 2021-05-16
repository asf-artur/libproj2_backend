using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using WebApplication2.Contracts;

namespace ConsoleApp1
{
    public class BookConverterService
    {
        public Book GetBookFromJObject(JObject jObject)
        {
            var author = jObject.Property("Автор")?.Value.ToString();
            var title = jObject.Property("Заглавие")?.Value.ToString();
            var publishersImprint = jObject.Property("Выходные данные")?.Value.ToString();
            var physicalDescription = jObject.Property("Физическое описание")?.Value.ToString();
            var volume = jObject.Property("Том")?.Value.ToString();
            var isbn = jObject.Property("ISBN")?.Value.ToString();
            var tagsString = jObject.Property("Тема")?.Value.ToString();
            var tags = tagsString?.Split(" -- ")
                .ToList();

            return new Book(
                author,
                title,
                publishersImprint,
                physicalDescription,
                volume,
                isbn,
                tags);
        }
    }
}