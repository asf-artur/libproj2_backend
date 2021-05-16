using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApplication2.Contracts.Books;
using WebApplication2.Db;

namespace WebApplication2.Repositories
{
    public class BookInfoRepository : DbRepository
    {
        private static BookInfo _bookInfo = new BookInfo(
            0,
            "Тетрадь",
            "в клетку",
            new [] {"Магазин \"Офисмаг\""}.ToList(),
            "офисмаг",
            DateTime.Parse("01.04.2018"),
            48,
            new List<string>(),
            "ru",
            "Тетради",
            IndustryIdentifier.DefaultIsbn("4606224157696"),
            "1.jpg",
            null);

        public async Task<List<BookInfo>> GetAll()
        {
            var query = "SELECT * FROM test.bookinfo";

            var result = (await Connection.QueryAsync<BookInfo>(query))
                .ToList();

            return result;
        }

        public BookInfo? Get(int id)
        {
            var query = "SELECT * FROM test.bookinfo WHERE id = :id";

            var result = Connection.QuerySingleOrDefault<BookInfo>(query, new {id});

            return result;
        }

        public async Task<List<BookInfo>> SearchBookInfoAsync(string searchTerm)
        {
            var query =
                @"select * from test.bookinfo b where lower(title) like :searchTerm or lower(authors) like :searchTerm or lower(subtitle) like :searchTerm";

            searchTerm = $"%{searchTerm}%";

            var result = await Connection.QueryAsync<BookInfo>(query, new {searchTerm});

            return result.ToList();
        }

        public async Task<List<BookInfo>> GetBookHistory(int userId)
        {
            var query = @"
SET schema 'test';
select * from bookinfo bi where bi.id
	in
	(
	select bc.bookinfo_id from bookcopy bc where bc.id
		in (
		select be.bookcopy_id from bookevent be where be.user_id = :user_id
		)
	)";

            var result = (await Connection.QueryAsync<BookInfo>(query, new { user_id = userId }))
                .ToList();

            return result;
        }
    }
}