using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class SchoolValidationException : Xeption
    {
        public SchoolValidationException(Xeption innerException)
            : base(message: "School validation errors occurred, please try again.",
                  innerException)
        { }
    }
}