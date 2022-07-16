using System;
using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class InvalidSchoolReferenceException : Xeption
    {
        public InvalidSchoolReferenceException(Exception innerException)
            : base(message: "Invalid school reference error occurred.", innerException) { }
    }
}