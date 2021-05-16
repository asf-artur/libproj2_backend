using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication2.Contracts.Books;

namespace WebApplication2.Services
{
    public class BookSearchService
    {
        private const string Path = "https://www.rsl.ru/";
        private readonly SeleniumGetSearchInfo _seleniumGetSearchInfo;
        private readonly WebApplication2.Services.BookConverterService _bookConverterService;

        public BookSearchService(SeleniumGetSearchInfo seleniumGetSearchInfo, WebApplication2.Services.BookConverterService bookConverterService)
        {
            _seleniumGetSearchInfo = seleniumGetSearchInfo;
            _bookConverterService = bookConverterService;
        }

        private async Task<List<BookInfo>> SearchInRslAsync(string searchTerm)
        {
            var jsonStrings = await _seleniumGetSearchInfo.GetAllFirstPageJsonsAsync(Path, searchTerm, CancellationToken.None);
            var jsons = jsonStrings.Select(c => JsonConvert.DeserializeObject(c) as JObject);
            var ids = Enumerable.Range(0, jsons.Count()).ToList();
            var result = jsons
                .Zip(ids)
                .Select(a => new{ JsonObject = a.First, Id = a.Second })
                .Select(c => _bookConverterService.GetBookFromJObject1(c.JsonObject, c.Id))
                .ToList();

            return result;
        }

        public async Task<List<BookInfo>> SearchInRslAsync(string searchTerm, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jsonStrings = await _seleniumGetSearchInfo.GetAllFirstPageJsonsAsync(Path, searchTerm, cancellationToken);
            var jsons = jsonStrings.Select(c => JsonConvert.DeserializeObject(c) as JObject);
            var ids = Enumerable.Range(0, jsons.Count()).ToList();
            var result = jsons
                .Zip(ids)
                .Select(a => new{ JsonObject = a.First, Id = a.Second })
                .Select(c => _bookConverterService.GetBookFromJObject1(c.JsonObject, c.Id))
                .ToList();

            return result;
        }
    }
}