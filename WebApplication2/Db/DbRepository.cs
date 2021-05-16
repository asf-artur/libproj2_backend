using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Newtonsoft.Json;
using NLog.Fluent;
using Npgsql;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Books;

namespace WebApplication2.Db
{
    public class DbRepository
    {
        protected readonly NpgsqlConnection Connection;
        private const string LocalDbString = "Server=localhost;port=5432;user id=postgres;password=postgres";
        private const string OracleDbString = "Server=152.70.52.35;port=5432;user id=web;password=web;database=postgres";

        public DbRepository()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new TemplateEnumTypeHandler());
            SqlMapper.AddTypeHandler(new TemplateListTypeHandler<string>());
            SqlMapper.AddTypeHandler(new TemplateListTypeHandler<IndustryIdentifier>());
            // Connection = new NpgsqlConnection(LocalDbString);
            Connection = new NpgsqlConnection(OracleDbString);
        }

        private class TemplateListTypeHandler<T> : SqlMapper.TypeHandler<List<T>>
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
        private class TemplateEnumTypeHandler : SqlMapper.TypeHandler<BookEventType>
        {
            public override void SetValue(IDbDataParameter parameter, BookEventType value)
            {
                parameter.Value = JsonConvert.SerializeObject(value);
            }

            public override BookEventType Parse(object value)
            {
                throw new NotImplementedException();
            }
        }
    }
}