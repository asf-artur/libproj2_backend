using System.Collections.Generic;
using WebApplication2.Contracts.Users;

namespace WebApplication2.Contracts.Notifications
{
    public class BookNotification: Notification
    {
        public int BookId { get; set; }

        public BookNotification(int id,
            string title,
            string text,
            List<UserCategory>? userCategoryIdList,
            List<int>? userIdList,
            NotificationType notificationType,
            int bookId)
            : base(id, title, text, userCategoryIdList, userIdList, notificationType)
        {
            BookId = bookId;
        }
    }
}