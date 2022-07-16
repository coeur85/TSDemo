using System;
using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class SchoolServiceException : Xeption
    {
        public SchoolServiceException(Exception innerException)
            : base(message: "School service error occurred, contact support.", innerException)
        { }
    }
}