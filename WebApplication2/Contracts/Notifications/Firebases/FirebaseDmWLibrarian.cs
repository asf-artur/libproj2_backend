using System.Collections.Generic;

namespace WebApplication2.Contracts.Notifications.Firebases
{
    public class FirebaseDmWLibrarian : FirebaseDm
    {
        private int _userLibrarianId;

        protected override Dictionary<string, string> AdditionalData => new Dictionary<string, string>
        {
            {"userLibrarianId", _userLibrarianId.ToString()}
        };

        public FirebaseDmWLibrarian(NotificationType notificationType, int initiatorUserId, int bookCopyId, int userLibrarianId)
            : base(notificationType, initiatorUserId, bookCopyId)
        {
            _userLibrarianId = userLibrarianId;
        }
    }
}