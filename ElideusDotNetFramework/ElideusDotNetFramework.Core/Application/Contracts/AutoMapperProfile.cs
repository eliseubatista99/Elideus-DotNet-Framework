using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace ElideusDotNetFramework.Core
{
    [ExcludeFromCodeCoverage]
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            this.CreateMapOfEnums();
            this.CreateMapOfEntities();
        }

        /// <summary>
        /// Create map of enums.
        /// </summary>
        protected virtual void CreateMapOfEnums()
        {

        }

        /// <summary>
        /// Create map of account entities.
        /// </summary>
        protected virtual void CreateMapOfEntities()
        {

        }
    }
}
