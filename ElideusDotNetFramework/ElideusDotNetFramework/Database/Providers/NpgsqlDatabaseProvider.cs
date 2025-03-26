using ElideusDotNetFramework.Application;
using Npgsql;

namespace ElideusDotNetFramework.Database
{
    public class NpgsqlDatabaseProvider<T> : INpgsqlDatabaseProvider<T>
    {
        protected IMapperProvider mapperProvider;

        protected string connectionString;

        public NpgsqlDatabaseProvider(IApplicationContext applicationContext)
        {
            this.mapperProvider = applicationContext.GetDependency<IMapperProvider>()!;
            connectionString = string.Empty;
        }

        public bool CreateTableIfNotExists()
        {
            return true;
        }

        public List<T> GetAll()
        {
            return new List<T>();
        }

        public T? GetById(string id)
        {
            return default;
        }

        public bool Add(T entry)
        {
            return true;
        }

        public bool Edit(T entry)
        {
            return true;
        }

        public bool Delete(string id)
        {
            return true;
        }

        public bool DeleteAll()
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
                            var dataEntry = mapperProvider.Map<NpgsqlDataReader, T>(sqlReader);

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
                        result = mapperProvider.Map<NpgsqlDataReader, T>(sqlReader);

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

        
    }
}
