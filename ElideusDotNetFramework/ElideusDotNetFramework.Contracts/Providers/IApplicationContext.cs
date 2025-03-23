﻿namespace ElideusDotNetFramework.Contracts.Providers
{
    public interface IApplicationContext
    {
        public void AddDependencies(List<object> _dependencies);

        public T? GetDependency<T>() where T : class;
    }
}
