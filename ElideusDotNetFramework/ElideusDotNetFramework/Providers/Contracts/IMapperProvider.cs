using AutoMapper;

namespace ElideusDotNetFramework.Providers.Contracts
{
    public interface IMapperProvider
    {
        public void CreateMapper(List<Profile> profiles);

        public Target Map<Source, Target>(Source source);

    }
}
