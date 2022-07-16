using Xeptions;

namespace TSDemo.Api.Models.Students.Exceptions
{
    public class StudentDependencyException : Xeption
    {
        public StudentDependencyException(Xeption innerException) :
            base(message: "Student dependency error occurred, contact support.", innerException)
        { }
    }
}