using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Contracts.Users
{
    public class RegisterUserData
    {
        public string Login { get; }

        [DataType(DataType.Password)]
        public string Password { get; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; }

        public RegisterUserData(string login, string password, string confirmPassword)
        {
            Login = login;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}