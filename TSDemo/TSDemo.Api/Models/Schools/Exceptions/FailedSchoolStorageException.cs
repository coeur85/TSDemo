using System;
using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class FailedSchoolStorageException : Xeption
    {
        public FailedSchoolStorageException(Exception innerException)
            : base(message: "Failed school storage error occurred, contact support.", innerException)
        { }
    }
}