using Xeptions;

namespace TSDemo.Api.Models.Students.Exceptions
{
    public class NullStudentException : Xeption
    {
        public NullStudentException()
            : base(message: "Student is null.")
        { }
    }
}