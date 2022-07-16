using System;
using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class AlreadyExistsSchoolException : Xeption
    {
        public AlreadyExistsSchoolException(Exception innerException)
            : base(message: "School with the same Id already exists.", innerException)
        { }
    }
}