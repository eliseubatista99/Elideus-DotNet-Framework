using AutoMapper;
using ElideusDotNetFramework.Providers.Contracts;

namespace ElideusDotNetFramework.Providers
{
    public class MapperProvider: IMapperProvider
    {
        private static IMapper? mapper;

        public Target Map<Source, Target>(Source source)
        {
            return mapper!.Map<Target>(source);
        }

        public void CreateMapper(List<Profile> profiles)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                //cfg.Advanced.AllowAdditiveTypeMapCreation = true;
                cfg.AddProfiles(profiles);
            });

            mapper = configuration.CreateMapper();
        }
    }
}
