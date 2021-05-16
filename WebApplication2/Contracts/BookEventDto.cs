using WebApplication2.Contracts.Books;

namespace WebApplication2.Contracts
{
    public class BookEventDto
    {
        public int BookId { get; set; }

        public int UserId { get; set; }

        public BookEvent BookEvent { get; set; }

        public BookEventDto(int bookId, int userId, BookEvent bookEvent)
        {
            BookId = bookId;
            UserId = userId;
            BookEvent = bookEvent;
        }
    }
}