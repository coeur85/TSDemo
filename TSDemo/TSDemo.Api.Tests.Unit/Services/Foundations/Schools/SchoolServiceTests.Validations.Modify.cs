using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TSDemo.Api.Models.Schools;
using TSDemo.Api.Models.Schools.Exceptions;
using Xunit;

namespace TSDemo.Api.Tests.Unit.Services.Foundations.Schools
{
    public partial class SchoolServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSchoolIsNullAndLogItAsync()
        {
            // given
            School nullSchool = null;
            var nullSchoolException = new NullSchoolException();

            var expectedSchoolValidationException =
                new SchoolValidationException(nullSchoolException);

            // when
            ValueTask<School> modifySchoolTask =
                this.schoolService.ModifySchoolAsync(nullSchool);

            SchoolValidationException actualSchoolValidationException =
                await Assert.ThrowsAsync<SchoolValidationException>(
                    modifySchoolTask.AsTask);

            // then
            actualSchoolValidationException.Should()
                .BeEquivalentTo(expectedSchoolValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSchoolAsync(It.IsAny<School>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfSchoolIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidSchool = new School
            {
                //Name = invalidText,
            };

            var invalidSchoolException = new InvalidSchoolException();

            invalidSchoolException.AddData(
                key: nameof(School.Id),
                values: "Id is required");

            //invalidSchoolException.AddData(
            //    key: nameof(School.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the School model

            invalidSchoolException.AddData(
                key: nameof(School.CreatedDate),
                values: "Date is required");

            invalidSchoolException.AddData(
                key: nameof(School.CreatedByUserId),
                values: "Id is required");

            invalidSchoolException.AddData(
                key: nameof(School.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(School.CreatedDate)}"
                });

            invalidSchoolException.AddData(
                key: nameof(School.UpdatedByUserId),
                values: "Id is required");

            var expectedSchoolValidationException =
                new SchoolValidationException(invalidSchoolException);

            // when
            ValueTask<School> modifySchoolTask =
                this.schoolService.ModifySchoolAsync(invalidSchool);

            SchoolValidationException actualSchoolValidationException =
                await Assert.ThrowsAsync<SchoolValidationException>(
                    modifySchoolTask.AsTask);

            //then
            actualSchoolValidationException.Should().BeEquivalentTo(expectedSchoolValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSchoolAsync(It.IsAny<School>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            School randomSchool = CreateRandomSchool(randomDateTimeOffset);
            School invalidSchool = randomSchool;
            var invalidSchoolException = new InvalidSchoolException();

            invalidSchoolException.AddData(
                key: nameof(School.UpdatedDate),
                values: $"Date is the same as {nameof(School.CreatedDate)}");

            var expectedSchoolValidationException =
                new SchoolValidationException(invalidSchoolException);

            // when
            ValueTask<School> modifySchoolTask =
                this.schoolService.ModifySchoolAsync(invalidSchool);

            SchoolValidationException actualSchoolValidationException =
                await Assert.ThrowsAsync<SchoolValidationException>(
                    modifySchoolTask.AsTask);

            // then
            actualSchoolValidationException.Should().BeEquivalentTo(expectedSchoolValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(invalidSchool.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}