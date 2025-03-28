using ElideusDotNetFramework.Core;
using ElideusDotNetFramework.Database;
using Npgsql;
using System.Diagnostics.CodeAnalysis;

namespace ElideusDotNetFramework.PostgreSql
{
    [ExcludeFromCodeCoverage]
    public class NpgsqlDatabaseProvider<T> : IDatabaseProvider<T>
    {
        protected IMapperProvider? mapperProvider;

        protected string connectionString;

        public NpgsqlDatabaseProvider(IApplicationContext applicationContext)
        {
            this.mapperProvider = applicationContext.GetDependency<IMapperProvider>();
            connectionString = string.Empty;
        }

        public virtual bool CreateTableIfNotExists()
        {
            return true;
        }

        public virtual List<T> GetAll()
        {
            return new List<T>();
        }

        public virtual T? GetById(string id)
        {
            return default;
        }

        public virtual bool Add(T entry)
        {
            return true;
        }

        public virtual bool Edit(T entry)
        {
            return true;
        }

        public virtual bool Delete(string id)
        {
            return true;
        }

        public virtual bool DeleteAll()
        {
            return true;
        }

        protected List<T> ExecuteReadMultiple(string connectionString, string command)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                List<T> result = new List<T>();

                connection.Open();

                var (transaction, commandExecutor) = NpgsqlDatabaseHelper.InitialzieSqlTransaction(connection);

                try
                {
                    commandExecutor.CommandText = command;

                    using (var sqlReader = commandExecutor.ExecuteReader())
                    {
                        while (sqlReader!.Read())
                        {
                            var dataEntry = MapDataReader(sqlReader)!;

                            result.Add(dataEntry);
                        }
                    }

                    return result;
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }

        protected T? ExecuteRead(string connectionString, string command)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                T? result = default;

                connection.Open();

                var (transaction, commandExecutor) = NpgsqlDatabaseHelper.InitialzieSqlTransaction(connection);

                try
                {
                    commandExecutor.CommandText = command;

                    var sqlReader = commandExecutor.ExecuteReader();

                    if (sqlReader.HasRows)
                    {
                        sqlReader.Read();
                        result = MapDataReader(sqlReader);

                    }

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        protected bool ExecuteWrite(string connectionString, string command)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var (transaction, commandExecutor) = NpgsqlDatabaseHelper.InitialzieSqlTransaction(connection);

                try
                {
                    commandExecutor.CommandText = command;

                    commandExecutor.ExecuteNonQuery();

                    transaction.Commit();

                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        protected virtual T? MapDataReader(NpgsqlDataReader dataReader)
        {
            if(mapperProvider == null)
            {
                return default;
            }

            return mapperProvider.Map<NpgsqlDataReader, T>(dataReader);
        }
    }
}
