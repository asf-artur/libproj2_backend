using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Npgsql;

namespace ConsoleApp1
{
    public enum UserCategory1
    {
        Admin = 0,

        Librarian = 1,

        Reader = 2,
    }
    public class User1
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserCategory1 UserCategory{ get; set; }

        public string? Barcode{ get; set; }

        public string? Rfid{ get; set; }

        public bool CanBorrowBooks{ get; set; }

        public DateTime RegistrationDate{ get; set; }

        public DateTime? LastVisitDate{ get; set; }

        public string? ImagePath{ get; set; }

        public string? ClientToken { get; set; }
    }
    public class DBClass
    {
        public int Id { get; set; }

        public int BookCopyId { get; set; }

        public int UserId { get; set; }

        public BookEventType1 BookEventType1 { get; set; }
        //
        // public string BookEventType1 => BookEventType1Enum.ToString();

        public DateTime Date { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BookEventType1
    {
        Borrowed,

        TryBorrow,

        Returned,
    }


    public class Db1
    {
        public static void Go()
        {
            var conn = new NpgsqlConnection("Server=localhost;port=5432;user id=postgres;password=postgres");
            // SqlMapper.AddTypeHandler(new ListTypeHandler());
            // SqlMapper.AddTypeHandler(new IndustryIdentifierTypeHandler());
            // SqlMapper.AddTypeHandler(new IndustryIdentifierCaTypeHandler());
            SqlMapper.AddTypeHandler(new TemplateTypeHandler<IndustryIdentifierCa>());
            Dapper.SqlMapper.AddTypeMap(typeof(BookEventType1), DbType.String);
            SqlMapper.AddTypeHandler(new TemplateTypeHandler<string>());
            // var books = conn.Query("Select * from test.bookinfo");
            // var books = conn.Query<BookInfo1>("Select * from test.bookinfo");
            var book = new BookInfoCa
            {
                Id = 1,
                Title = "title",
                SubTitle = "fdgfg",
                Authors = new List<string>{ "a" },
                PublishedDate = DateTime.Now,
                IndustryIdentifiers = new List<IndustryIdentifierCa>
                {
                    new IndustryIdentifierCa(IndustryIdentifierType.ISBN_13, "12124325")
                }
            };


            var sql = $@"
INSERT INTO test.bookinfo
(
title,
PublishedDate,
Authors,
IndustryIdentifiers
)
VALUES
(
:title,
:PublishedDate,
:Authors,
:IndustryIdentifiers
)
".Replace("\r","")
.Replace("\n","");

            var bookevent = new DBClass
            {
                UserId = 3,
                BookCopyId = 3,
                BookEventType1 = BookEventType1.Borrowed,
                Date = DateTime.Now,
            };

            var jj = JsonConvert.SerializeObject(bookevent);

            var query = "update test.\"user\" set client_token = :clientToken where id = :id";
            var clientToken = "tttt";
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            var users = conn.Query<User1>("select * from test.\"user\"").ToList();

//             conn.Execute(@"insert into test.bookevent
//             (
// bookcopy_id,
// user_id,
// book_event_type,
// ""date""
// ) values
//     (
// :BookCopyId,
// :UserId,
// :BookEventType1,
// :Date)",
//                 bookevent);

            var ind = new IndustryIdentifierCa(IndustryIdentifierType.ISBN_13, "9781840224979");
            var j = JsonConvert.SerializeObject(new List<IndustryIdentifierCa>{ind});
            // conn.Execute(sql, book);

            // var res = conn.Query<BookInfoCa>("select * from test.bookinfo where IndustryIdentifiers is not null");

            conn.Close();

            var q = 0;
        }
    }

    public class TemplateTypeHandler<T> : SqlMapper.TypeHandler<List<T>>
    {
        public override void SetValue(IDbDataParameter parameter, List<T> value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override List<T> Parse(object value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                var s = value as string;
                var result = JsonConvert.DeserializeObject<List<T>>(s);

                return result;
            }
            catch
            {
                return null;
            }
        }
    }

    public class IndustryIdentifierCaTypeHandler : SqlMapper.TypeHandler<IndustryIdentifierCa>
    {
        public override void SetValue(IDbDataParameter parameter, IndustryIdentifierCa value)
        {
            var result = JsonConvert.SerializeObject(value);
            parameter.Value = result;
        }

        public override IndustryIdentifierCa Parse(object value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                var s = value as string;
                var result = JsonConvert.DeserializeObject<IndustryIdentifierCa>(s);

                return result;
            }
            catch
            {
                return null;
            }
        }
    }

    public class ListTypeHandler : SqlMapper.TypeHandler<List<string>>
    {
        public override void SetValue(IDbDataParameter parameter, List<string> value)
        {
            parameter.Value = value.Aggregate((a, b) => $"{a},{b}");
        }

        public override List<string> Parse(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is string s)
            {
                var result = s.Split(',')
                    .Select(c => c.Trim())
                    .ToList();

                return result;
            }


            return null;
        }
    }

    public class IndustryIdentifierListTypeHandler : SqlMapper.TypeHandler<List<IndustryIdentifierCa>>
    {
        public override void SetValue(IDbDataParameter parameter, List<IndustryIdentifierCa> value)
        {
            var result = JsonConvert.SerializeObject(value);
            parameter.Value = result;
        }

        public override List<IndustryIdentifierCa> Parse(object value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                var s = value as string;
                var result = JsonConvert.DeserializeObject<List<IndustryIdentifierCa>>(s);

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}