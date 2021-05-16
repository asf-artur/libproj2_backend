using System.Collections.Generic;
using System.Linq;

namespace WebApplication2.Contracts.Notifications.Firebases
{
    public class FirebaseDm
    {
        private NotificationType _notificationType;

        private int _initiatorUserId;

        private int _bookCopyId;

        protected Dictionary<string, string> BaseData => new Dictionary<string, string>
        {
            { "notificationType", _notificationType.ToString() },
            { "bookCopyId", _bookCopyId.ToString() },
            { "initiatorUserId", _initiatorUserId.ToString() },
        };

        protected virtual Dictionary<string, string> AdditionalData => new Dictionary<string, string>();

        public Dictionary<string, string> Data
        {
            get
            {
                var result = new Dictionary<string, string>();

                BaseData
                    .Union(AdditionalData.ToList())
                    .ToList()
                    .ForEach(tuple => result.Add(tuple.Key, tuple.Value));

                return result;
            }
        }

        public FirebaseDm(NotificationType notificationType, int initiatorUserId, int bookCopyId)
        {
            _notificationType = notificationType;
            _initiatorUserId = initiatorUserId;
            _bookCopyId = bookCopyId;
        }
    }
}