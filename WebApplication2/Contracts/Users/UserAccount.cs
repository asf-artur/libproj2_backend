using System.ComponentModel;

namespace WebApplication2.Contracts.Users
{
    public class UserAccount
    {
        public int UserId { get; }

        public LoginUserData LoginUserData { get; }

        public UserAccount(int userId, LoginUserData loginUserData)
        {
            UserId = userId;
            LoginUserData = loginUserData;
        }
    }
}