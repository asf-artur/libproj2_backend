using System.Collections.Generic;
using System.Linq;
using Dapper;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Books;
using WebApplication2.Db;

namespace WebApplication2.Repositories
{
    public class BookEventRepository : DbRepository
    {
        public List<BookEvent> GetAll()
        {
            var query = "select * from test.bookevent order by date desc";

            var result = Connection.Query<BookEvent>(query)
                .ToList();

            return result;
        }

        public void Add(BookEvent bookEvent)
        {
            var query = $@"
insert into test.bookevent
(
bookcopy_id,
user_id,
book_event_type,
""date""
) values 
(
:BookCopyId,
:UserId,
:BookEventTypeString,
:Date)
";
            Connection.Execute(query, bookEvent);
        }
    }
}