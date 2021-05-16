using System;

namespace WebApplication2.Exceptions
{
    public class UserNotFoundException
        : Exception
    {
        public UserNotFoundException()
            : base("User not found")
        {
        }
    }
}