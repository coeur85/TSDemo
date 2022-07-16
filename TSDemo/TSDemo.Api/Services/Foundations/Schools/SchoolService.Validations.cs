using System;
using TSDemo.Api.Models.Schools;
using TSDemo.Api.Models.Schools.Exceptions;

namespace TSDemo.Api.Services.Foundations.Schools
{
    public partial class SchoolService
    {
        private void ValidateSchoolOnAdd(School school)
        {
            ValidateSchoolIsNotNull(school);

            Validate(
                (Rule: IsInvalid(school.Id), Parameter: nameof(School.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(school.CreatedDate), Parameter: nameof(School.CreatedDate)),
                (Rule: IsInvalid(school.CreatedByUserId), Parameter: nameof(School.CreatedByUserId)),
                (Rule: IsInvalid(school.UpdatedDate), Parameter: nameof(School.UpdatedDate)),
                (Rule: IsInvalid(school.UpdatedByUserId), Parameter: nameof(School.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: school.UpdatedDate,
                    secondDate: school.CreatedDate,
                    secondDateName: nameof(School.CreatedDate)),
                Parameter: nameof(School.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: school.UpdatedByUserId,
                    secondId: school.CreatedByUserId,
                    secondIdName: nameof(School.CreatedByUserId)),
                Parameter: nameof(School.UpdatedByUserId)));
        }

        private static void ValidateSchoolIsNotNull(School school)
        {
            if (school is null)
            {
                throw new NullSchoolException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidSchoolException = new InvalidSchoolException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSchoolException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSchoolException.ThrowIfContainsErrors();
        }
    }
}