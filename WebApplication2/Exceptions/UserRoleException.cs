using System;

namespace WebApplication2.Exceptions
{
    public class UserRoleException
        : Exception
    {
        public UserRoleException()
            : base("User role error")
        {

        }
    }
}