using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Users;
using WebApplication2.Db;
using WebApplication2.Exceptions;

namespace WebApplication2.Repositories
{
    public class UserRepository : DbRepository
    {
        public List<User> AllUsers { get; }

        public static User UserMe = new User(
            0,
            "Артур Асфандияров",
            UserCategory.Admin,
            "1234567890123",
            null,
            true,
            DateTime.Today,
            DateTime.Today,
            null,
            ""
        );

        public static List<User> Default = new List<User>
        {
            UserMe,
            new User(
                1,
                "Библиотекарь",
                UserCategory.Librarian,
                "1234567890123",
                null,
                true,
                DateTime.Today,
                DateTime.Today,
                null,
                "e-sI-s0rQ0Swe13jL8ntTb:APA91bG7JuZ9w2dnUJhssWSi-D3ZHiCc-uOXG1PHJ0mzMcpMxZnn4DMKydEFgI_6-VwQ5GFIPEjyO2Z37MVpQzgw5amCCgI6e1w_H9i8Iw_GkOpJ17PeGdzPW4E_IYk4nVo5Cf796VD4"
                ),
            new User(
                2,
                "Читатель",
                UserCategory.Reader,
                "1234567890123",
                null,
                true,
                DateTime.Today,
                DateTime.Today,
                null,
                null
            )
        };

        public UserRepository()
        {
            AllUsers = new List<User>();
        }

        public void SetClientToken(int userId, string? clientToken)
        {
            var query = "update test.\"user\" set client_token = :client_token where id = :id";

            Connection.Execute(query, new {id = userId, client_token = clientToken});
        }

        public async Task<User?> Get(int id)
        {
            var query = "SELECT * FROM test.\"user\" WHERE id = :id limit 1";

            var user = await Connection.QueryFirstOrDefaultAsync<User>(query, new { id });

            if (!AllUsers.Contains(user))
            {
                AllUsers.Add(user);
            }

            return user;
        }

        public List<User> GetAll()
        {
            var query = "SELECT * FROM test.\"user\"";

            var users = Connection.Query<User>(query)
                .ToList();

            return users;
        }

        public List<User> Search(string searchTerm)
        {
            searchTerm = $"%{searchTerm}%";
            var query = "select * from test.\"user\" u where lower(\"name\") like :searchTerm or barcode = :searchTerm";

            var user = Connection.Query<User>(query, new { searchTerm });

            return user.ToList();
        }

        public User? FindByBarcode(string barcode)
        {
            var query = "SELECT * FROM test.\"user\" WHERE barcode = :barcode limit 1";

            var user = Connection.QueryFirstOrDefault<User>(query, new { barcode });

            if (!AllUsers.Contains(user))
            {
                AllUsers.Add(user);
            }

            return user;
        }

        public List<User> GetAllLibrarians()
        {
            var query = "SELECT * FROM test.\"user\" WHERE user_category in ('Librarian', 'Admin')";

            var result = Connection.Query<User>(query)
                .ToList();

            if (result.Count != 0)
            {
                return result;
            }
            else
            {
                throw new UserNotFoundException();
            }
        }
    }
}