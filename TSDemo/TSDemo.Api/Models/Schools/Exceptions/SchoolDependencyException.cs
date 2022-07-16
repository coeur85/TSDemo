using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class SchoolDependencyException : Xeption
    {
        public SchoolDependencyException(Xeption innerException) :
            base(message: "School dependency error occurred, contact support.", innerException)
        { }
    }
}