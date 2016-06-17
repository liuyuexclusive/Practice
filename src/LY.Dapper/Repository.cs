using LY.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Reflection;
using Dapper;
using System.Text;

namespace LY.Dapper
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private string ConnectString
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("DefaultConnection");
            }
        }

        private string TableName
        {
            get
            {
                var type = typeof(TEntity);
                return (type.Namespace.TrimStart("LY.Domain.".ToArray()) + "_" + type.Name).ToLower();
            }
        }

        public DbConnection CreateConnection()
        {
            var conn = new MySql.Data.MySqlClient.MySqlConnection(ConnectString);
            return conn;
        }

        public IEnumerable<TEntity> List()
        {
            var type = typeof(TEntity);
            IEnumerable<string> fields = type.GetProperties().Select(a => a.Name.ToLower());
            var sql = "select " + string.Join(",", fields) + " from " + TableName;
            using (var conn = CreateConnection())
            {
                return conn.Query<TEntity>(sql);
            }
        }

        public int Add(TEntity entity)
        {
            var type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var propName = prop.Name.ToLower();
                if (string.Equals(propName, "id"))
                {
                    continue;
                }

                dic.Add("`" + prop.Name.ToLower() + "`", "\"" + prop.GetValue(entity) + "\"");
            }
            var names = string.Join(",", dic.Keys);
            var values = string.Join(",", dic.Values);

            var sql = "insert into " + TableName + "(" + names + ") values " + "(" + values + ");";

            using (var conn = CreateConnection())
            {
                return conn.Execute(sql);
            }
        }

        public void Adds(IList<TEntity> list)
        {
            foreach (var entity in list)
            {
                Add(entity);
            }
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
