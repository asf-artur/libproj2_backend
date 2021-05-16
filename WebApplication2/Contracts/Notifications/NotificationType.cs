namespace WebApplication2.Contracts.Notifications
{
    public enum NotificationType
    {
        // Важное
        Important = 0,

        // Книгу берут в процессе
        TryBookBorrow = 1,

        // Книга взята
        BookIsBorrowed = 2,

        // Книга поступила
        BookArrived = 3,

        BookReturnLastDay = 4,

        BookReturnOneDayLeft = 5,

        BookReturnPeriodExpired = 6,

        TryBookBorrowFailure = 7,

        BookReturned = 9,

        // Пока не будет
        News = 100,
    }
}