namespace ElideusDotNetFramework.Core.Errors
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
