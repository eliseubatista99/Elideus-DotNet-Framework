using ElideusDotNetFramework.Providers.Contracts;

namespace ElideusDotNetFramework.Providers
{
    public class ApplicationContext : IApplicationContext
    {
        private List<object> dependencies = new List<object>();

        public void AddDependencies(List<object> _dependencies)
        {
            for(int i = 0; i < _dependencies.Count; i++)
            {
                var existingDependencyIndex = _dependencies.FindIndex(d => d.Equals(_dependencies[i]));

                // If the dependency is already added, override it
                if (existingDependencyIndex != -1)
                {
                    dependencies[existingDependencyIndex] = _dependencies[i];
                }
                else
                {
                    dependencies.Add(_dependencies[i]);
                }
            }
        }

        public T? GetDependency<T>() where T : class
        {
            foreach (var dependency in dependencies)
            {
                if (dependency is T)
                {
                    return (T)dependency;
                }
            }

            return default(T);
        }
    }
}
