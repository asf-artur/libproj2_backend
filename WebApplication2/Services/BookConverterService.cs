using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using WebApplication2.Contracts.Books;

namespace WebApplication2.Services
{
    public class BookConverterService
    {
        // private BookInfo GetBookFromJObject(JObject jObject, int id)
        // {
        //     var author = jObject.Property("Автор")?.Value.ToString();
        //     var title = jObject.Property("Заглавие")?.Value.ToString();
        //     var publishersImprint = jObject.Property("Выходные данные")?.Value.ToString();
        //     var physicalDescription = jObject.Property("Физическое описание")?.Value.ToString();
        //     var volume = jObject.Property("Том")?.Value.ToString();
        //     var isbn = jObject.Property("ISBN")?.Value.ToString();
        //     var tagsString = jObject.Property("Тема")?.Value.ToString();
        //     var tags = tagsString?.Split(" -- ")
        //         .ToList();
        //
        //     return new BookInfo(
        //         id,
        //         new List<string>{ author },
        //         title,
        //         publishersImprint,
        //         physicalDescription,
        //         volume,
        //         isbn,
        //         tags);
        // }

        public BookInfo GetBookFromJObject1(JObject jObject, int id)
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

            return new BookInfo(
                id,
                title,
                volume,
                new List<string>{ author },
                publishersImprint,
                null,
                -1,
                tags,
                "ru",
                "",
                IndustryIdentifier.DefaultIsbn(isbn),
                null,
                null);
        }
    }
}