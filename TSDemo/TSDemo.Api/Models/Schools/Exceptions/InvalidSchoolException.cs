using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class InvalidSchoolException : Xeption
    {
        public InvalidSchoolException()
            : base(message: "Invalid school. Please correct the errors and try again.")
        { }
    }
}