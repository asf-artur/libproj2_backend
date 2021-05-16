using System.Collections.Generic;
using WebApplication2.Contracts.Users;

namespace WebApplication2.Contracts.Notifications
{
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public List<UserCategory>? UserCategoryIdList { get; set; }

        public List<int>? UserIdList { get; set; }

        public NotificationType NotificationType { get; set; }

        public Dictionary<string, string> Data => new Dictionary<string, string>
        {

        };

        public Notification(int id, string title, string text, List<UserCategory>? userCategoryIdList, List<int>? userIdList, NotificationType notificationType)
        {
            Id = id;
            Title = title;
            Text = text;
            UserCategoryIdList = userCategoryIdList;
            UserIdList = userIdList;
            NotificationType = notificationType;
        }
    }
}