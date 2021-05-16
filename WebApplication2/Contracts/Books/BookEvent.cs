using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApplication2.Contracts.Books
{
    public class BookEvent
    {
        public int Id { get; set; }

        public int BookCopyId { get; set; }

        public int UserId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public BookEventType BookEventType { get; set; }

        public string BookEventTypeString => BookEventType.ToString();

        public DateTime Date { get; set; }

        public BookEvent()
        {
        }

        public BookEvent(int bookCopyId, int userId, BookEventType bookEventType, DateTime date)
        {
            BookCopyId = bookCopyId;
            UserId = userId;
            BookEventType = bookEventType;
            Date = date;
        }
    }
}