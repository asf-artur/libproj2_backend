namespace WebApplication2.Contracts.Books
{
    public enum BookEventType
    {
        Borrowed,

        TryBorrow,

        Returned,

        Lost,

        WrittenOff,

        ReturnExpired,

        Extended,

        AddedNew,

        LibrarianAcceptedBorrow,

        LibrarianRejectedBorrow,

        LibrarianAcceptedReturn,

        Rejected,
    }
}