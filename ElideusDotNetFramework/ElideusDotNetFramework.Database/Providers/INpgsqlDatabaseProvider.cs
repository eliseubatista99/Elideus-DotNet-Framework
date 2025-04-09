namespace ElideusDotNetFramework.Database
{
    public interface IDatabaseProvider<T>
    {
        public bool CreateTableIfNotExists();

        public List<T> GetAll();

        public T? GetById(string id);

        public bool Add(T entry);

        public bool Edit(T entry);

        public bool Delete(string id);

        public bool DeleteAll();
    }
}
