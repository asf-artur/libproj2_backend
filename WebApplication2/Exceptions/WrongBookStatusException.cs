using System;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Books;

namespace WebApplication2.Exceptions
{
    public class WrongBookStatusException
        : Exception
    {
        public WrongBookStatusException(BookStatus current)
            : base($"Неверный статус книги: {current}")
        {
        }
    }
}