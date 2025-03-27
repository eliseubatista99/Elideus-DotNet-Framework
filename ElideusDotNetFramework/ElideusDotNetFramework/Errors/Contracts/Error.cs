using System.Diagnostics.CodeAnalysis;

namespace ElideusDotNetFramework.Core.Errors
{
    [ExcludeFromCodeCoverage]
    public class Error
    {
        public required string Code { get; set; }

        public required string Message { get; set; }
    }
}
