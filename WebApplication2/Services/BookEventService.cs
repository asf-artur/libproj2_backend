using System;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Books;
using WebApplication2.Repositories;

namespace WebApplication2.Services
{
    public class BookEventService
    {
        private readonly BookEventRepository _bookEventRepository;

        public BookEventService(BookEventRepository bookEventRepository)
        {
            _bookEventRepository = bookEventRepository;
        }

        public void TryBorrowBook(int bookId, int userReaderId)
        {
            _bookEventRepository.Add(new BookEvent(bookId, userReaderId, BookEventType.TryBorrow, DateTime.Now));
        }

        public void RejectBorrowBook(int bookId, int userReaderId, int userLibrarianId)
        {
            _bookEventRepository.Add(new BookEvent(bookId, userReaderId, BookEventType.Rejected, DateTime.Now));
            _bookEventRepository.Add(new BookEvent(bookId, userLibrarianId, BookEventType.LibrarianRejectedBorrow, DateTime.Now));
        }

        public void CompleteBorrowBook(int bookId, int userReaderId, int userLibrarianId)
        {
            _bookEventRepository.Add(new BookEvent(bookId, userReaderId, BookEventType.Borrowed, DateTime.Now));
            _bookEventRepository.Add(new BookEvent(bookId, userLibrarianId, BookEventType.LibrarianAcceptedBorrow, DateTime.Now));
        }

        public void ReturnBook(int bookId, int userReaderId, int userLibrarianId)
        {
            _bookEventRepository.Add(new BookEvent(bookId, userReaderId, BookEventType.Returned, DateTime.Now));
            _bookEventRepository.Add(new BookEvent(bookId, userLibrarianId, BookEventType.LibrarianAcceptedReturn, DateTime.Now));
        }
    }
}