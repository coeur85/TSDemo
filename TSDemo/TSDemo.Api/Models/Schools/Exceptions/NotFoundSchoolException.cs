using System;
using Xeptions;

namespace TSDemo.Api.Models.Schools.Exceptions
{
    public class NotFoundSchoolException : Xeption
    {
        public NotFoundSchoolException(Guid schoolId)
            : base(message: $"Couldn't find school with schoolId: {schoolId}.")
        { }
    }
}