using System;
using System.Threading.Tasks;
using ConsoleApp1;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Books;
using WebApplication2.Exceptions;
using WebApplication2.Repositories;

namespace WebApplication2.Services
{
    public class BookBorrowService
    {
        public const int BorrowDays = 5;

        private readonly UserRepository _userRepository;
        private readonly BookCopyRepository _bookCopyRepository;
        private readonly NotificationService _notificationService;
        private readonly BookEventService _bookEventService;


        public BookBorrowService(UserRepository userRepository,
            BookCopyRepository bookCopyRepository,
            NotificationService notificationService,
            BookEventService bookEventService)
        {
            _userRepository = userRepository;
            _bookCopyRepository = bookCopyRepository;
            _notificationService = notificationService;
            _bookEventService = bookEventService;
        }

        public async Task TryBorrowBook(int bookId, int userId)
        {
            var bookCopy = _bookCopyRepository.Get(bookId);
            var user = await _userRepository.Get(userId);

            if (bookCopy != null && user != null)
            {
                if (bookCopy.BookStatus == BookStatus.NotInStock)
                {
                    throw new WrongBookStatusException(bookCopy.BookStatus);
                }

                _bookEventService.TryBorrowBook(bookId, userId);
                _notificationService.TryBorrowBookNotification(bookCopy, user);
            }
        }

        public async Task RejectBorrowBook(int bookId, int userReaderId, int userLibrarianId)
        {
            var bookCopy = _bookCopyRepository.Get(bookId);
            var userReader = await _userRepository.Get(userReaderId);
            var userLibrarian = await _userRepository.Get(userLibrarianId);

            if (bookCopy != null && userReader != null && userLibrarian != null)
            {
                if (bookCopy.BookStatus == BookStatus.NotInStock)
                {
                    throw new WrongBookStatusException(bookCopy.BookStatus);
                }

                _bookEventService.RejectBorrowBook(bookId, userReaderId, userLibrarianId);
                _notificationService.RejectBorrowBookNotification(bookCopy, userReader, userLibrarian);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task CompleteBorrowBook(int bookId, int userReaderId, int userLibrarianId)
        {
            var bookCopy = _bookCopyRepository.Get(bookId);
            var userReader = await _userRepository.Get(userReaderId);
            var userLibrarian = await _userRepository.Get(userLibrarianId);

            if (bookCopy != null && userReader != null && userLibrarian != null)
            {
                if (bookCopy.BookStatus == BookStatus.NotInStock)
                {
                    throw new WrongBookStatusException(bookCopy.BookStatus);
                }

                bookCopy.BookStatus = BookStatus.NotInStock;
                bookCopy.UserId = userReaderId;
                bookCopy.ReturnDate = DateTime.Now.Add(TimeSpan.FromDays(BorrowDays));
                _bookCopyRepository.BorrowBook(bookCopy, userReader);
                _bookEventService.CompleteBorrowBook(bookId, userReaderId, userLibrarianId);
                _notificationService.CompleteBorrowBookNotification(bookCopy, userReader, userLibrarian);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task ReturnBook(int bookId, int userReaderId, int userLibrarianId)
        {
            var bookCopy = _bookCopyRepository.Get(bookId);
            var userReader = await _userRepository.Get(userReaderId);
            var userLibrarian = await _userRepository.Get(userLibrarianId);

            if (bookCopy != null && userReader != null && userLibrarian != null)
            {
                if (bookCopy.BookStatus == BookStatus.InStock)
                {
                    throw new WrongBookStatusException(bookCopy.BookStatus);
                }

                bookCopy.BookStatus = BookStatus.InStock;
                bookCopy.UserId = null;
                bookCopy.ReturnDate = null;
                _bookCopyRepository.ReturnBook(bookCopy, userReader);
                _bookEventService.ReturnBook(bookId, userReaderId, userLibrarianId);
                _notificationService.ReturnBookNotification(bookCopy, userReader, userLibrarian);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}