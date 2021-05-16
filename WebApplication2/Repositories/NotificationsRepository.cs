using System.Collections.Generic;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Notifications;

namespace WebApplication2.Repositories
{
    public class NotificationsRepository
    {
        private List<Notification> _notifications;

        public NotificationsRepository()
        {
            _notifications = new List<Notification>();
        }

        public void Add(Notification notification)
        {
            _notifications.Add(notification);
        }

        public List<Notification> GetAll()
        {
            return _notifications;
        }
    }
}