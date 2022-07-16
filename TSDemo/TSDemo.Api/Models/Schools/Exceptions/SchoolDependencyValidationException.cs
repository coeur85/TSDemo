using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class SchoolDependencyValidationException : Xeption
    {
        public SchoolDependencyValidationException(Xeption innerException)
            : base(message: "School dependency validation occurred, please try again.", innerException)
        { }
    }
}