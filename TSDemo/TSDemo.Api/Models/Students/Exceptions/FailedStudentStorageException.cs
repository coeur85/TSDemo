using System;
using Xeptions;

namespace TSDemo.Api.Models.Students.Exceptions
{
    public class FailedStudentStorageException : Xeption
    {
        public FailedStudentStorageException(Exception innerException)
            : base(message: "Failed student storage error occurred, contact support.", innerException)
        { }
    }
}