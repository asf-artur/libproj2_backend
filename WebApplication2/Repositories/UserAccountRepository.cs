using System.Collections.Generic;
using System.Linq;
using Dapper;
using WebApplication2.Contracts.Users;
using WebApplication2.Db;

namespace WebApplication2.Repositories
{
    public class UserAccountRepository : DbRepository
    {
        private static List<UserAccount> Default => new List<UserAccount>
        {
            new UserAccount(0, new LoginUserData("a","a")),
            new UserAccount(1, new LoginUserData("b","b")),
            new UserAccount(2, new LoginUserData("c","c")),
        };

        public int? CheckLogin(LoginUserData loginUserData)
        {
            var query = "SELECT user_id FROM test.user_account WHERE login = :login and passwd = :passwd LIMIT 1";
            var userId = Connection.QuerySingleOrDefault<int?>(query, new {login = loginUserData.Login, passwd = loginUserData.Password});

            return userId;

        }
    }
}