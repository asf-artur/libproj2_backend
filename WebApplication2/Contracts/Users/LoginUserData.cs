namespace WebApplication2.Contracts.Users
{
    public class LoginUserData
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public LoginUserData()
        {
        }

        public LoginUserData(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}