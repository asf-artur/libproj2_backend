using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Books;
using WebApplication2.Contracts.Users;
using WebApplication2.Db;

namespace WebApplication2.Repositories
{
    public class BookCopyRepository : DbRepository
    {
        private static BookInfo _bookInfo = new BookInfo(
            0,
            "Тетрадь",
            "в клетку",
            new [] {"Магазин \"Офисмаг\""}.ToList(),
            "офисммаг",
            DateTime.Parse("01.04.2018"),
            48,
            new List<string>(),
            "ru",
            "Тетради",
            IndustryIdentifier.DefaultIsbn("4606224157696"),
            null,
            null);

        public static BookCopy MyBook = new BookCopy(
            0,
            _bookInfo,
            BookStatus.InStock,
            "4606224157696",
            null,
            null,
            null
            );

        public BookCopy? Get(int id)
        {
            var query = "SELECT * FROM test.bookcopy bc join test.bookinfo bi on bc.bookinfo_id = bi.id WHERE bc.id = :id";


            // TODO: Single or default ??
            var result = Connection.Query<BookCopy, BookInfo, BookCopy>(query,
                    (bookCopy, bookInfo) =>
                    {
                        bookCopy.BookInfo = bookInfo;
                        return bookCopy;
                    }, new { id })
                .FirstOrDefault();

            return result;
        }

        public async Task<List<BookCopy>> GetAll()
        {
            var query = "SELECT * FROM test.bookcopy bc join test.bookinfo bi on bc.bookinfo_id = bi.id";

            var result = (await Connection.QueryAsync<BookCopy, BookInfo, BookCopy>(query,
                (bookCopy, bookInfo) =>
                {
                    bookCopy.BookInfo = bookInfo;
                    return bookCopy;
                }))
                .ToList();

            return result;
        }

        public async Task<List<BookCopy>> SearchBookCopyAsync(string searchTerm)
        {
            var query = @"SELECT * FROM test.bookcopy bc join test.bookinfo bi on bc.bookinfo_id = bi.id
where lower(bi.title) like :searchTerm
        or lower(bi.authors) like :searchTerm
        or lower(bi.subtitle) like :searchTerm";

            searchTerm = $"%{searchTerm}%";

            var result = await Connection.QueryAsync<BookCopy, BookInfo, BookCopy>(query,
                (bookCopy, bookInfo) =>
                {
                    bookCopy.BookInfo = bookInfo;
                    return bookCopy;
                }, new {searchTerm});

            return result.ToList();
        }

        public BookCopy? SearchByBarcode(string barcode)
        {
            var query = "SELECT * FROM test.bookcopy bc join test.bookinfo bi on bc.bookinfo_id = bi.id where bc.barcode = :barcode limit 1";

            var result = Connection.Query<BookCopy, BookInfo, BookCopy>(query,
                    (bookCopy, bookInfo) =>
                    {
                        bookCopy.BookInfo = bookInfo;
                        return bookCopy;
                    }, new { barcode })
                .FirstOrDefault();

            return result;
        }

        public BookCopy? SearchByRfid(string rfid)
        {
            var query = "SELECT * FROM test.bookcopy bc join test.bookinfo bi on bc.bookinfo_id = bi.id where bc.rfid = :rfid limit 1";

            var result = Connection.Query<BookCopy, BookInfo, BookCopy>(query,
                    (bookCopy, bookInfo) =>
                    {
                        bookCopy.BookInfo = bookInfo;
                        return bookCopy;
                    }, new { rfid })
                .FirstOrDefault();

            return result;
        }

        public async Task<List<BookCopy>> GetOnHands(int userId)
        {
            var query = "SELECT * FROM test.bookcopy bc join test.bookinfo bi on bc.bookinfo_id = bi.id where user_id = :user_id";

            var result = await Connection.QueryAsync<BookCopy, BookInfo, BookCopy>(query,
                (bookCopy, bookInfo) =>
                {
                    bookCopy.BookInfo = bookInfo;
                    return bookCopy;
                }, new {user_id = userId});

            return result.ToList();
        }

        public void BorrowBook(BookCopy bookCopy, User user)
        {
            var query = "update test.bookcopy set user_id = :user_id, book_status = :book_status, return_date = :return_date where id = :id";

            Connection.Execute(query,
                new
                {
                    user_id = user.Id,
                    book_status = bookCopy.BookStatus,
                    return_date = bookCopy.ReturnDate,
                    id = bookCopy.Id
                });
        }

        public void ReturnBook(BookCopy bookCopy, User user)
        {
            var query = "update test.bookcopy set user_id = null, book_status = :book_status, return_date = null where id = :id";

            Connection.Execute(query,
                new
                {
                    book_status = bookCopy.BookStatus,
                    id = bookCopy.Id
                });
        }
    }
}