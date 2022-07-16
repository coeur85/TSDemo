using System;
using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class FailedSchoolServiceException : Xeption
    {
        public FailedSchoolServiceException(Exception innerException)
            : base(message: "Failed school service occurred, please contact support", innerException)
        { }
    }
}