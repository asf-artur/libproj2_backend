using System;

namespace WebApplication2.Contracts.Books
{
    public class BookCopy
    {
        public int Id { get; set; }

        public BookInfo BookInfo { get; set; }

        public BookStatus BookStatus { get; set; }

        public string? Barcode { get; set; }

        public string? Rfid { get; set; }

        public int? UserId { get; set; }

        public DateTimeOffset? ReturnDate { get; set; }

        public BookCopy()
        {
        }

        public BookCopy(int id, BookInfo bookInfo, BookStatus bookStatus, string? barcode, string? rfid, int? userId, DateTimeOffset? returnDate)
        {
            Id = id;
            BookInfo = bookInfo;
            BookStatus = bookStatus;
            Barcode = barcode;
            Rfid = rfid;
            UserId = userId;
            ReturnDate = returnDate;
        }
    }
}