using ElideusDotNetFramework.Errors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElideusDotNetFramework.Errors
{
    public class InputErrors 
    {
        public static Error InvalidInput = new Error { Code = "InvalidInput", Message = "Invalid Input" };

        public static Error InvalidInputField(string field)
        {
            return new Error { Code = "InvalidInput", Message = $"Invalid Input: {field}" };
        }
    }
}
