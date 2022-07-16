using System;
using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class LockedSchoolException : Xeption
    {
        public LockedSchoolException(Exception innerException)
            : base(message: "Locked school record exception, please try again later", innerException)
        {
        }
    }
}