using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class NullSchoolException : Xeption
    {
        public NullSchoolException()
            : base(message: "School is null.")
        { }
    }
}