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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidSchoolId = Guid.Empty;

            var invalidSchoolException =
                new InvalidSchoolException();

            invalidSchoolException.AddData(
                key: nameof(School.Id),
                values: "Id is required");

            var expectedSchoolValidationException =
                new SchoolValidationException(invalidSchoolException);

            // when
            ValueTask<School> removeSchoolByIdTask =
                this.schoolService.RemoveSchoolByIdAsync(invalidSchoolId);

            SchoolValidationException actualSchoolValidationException =
                await Assert.ThrowsAsync<SchoolValidationException>(
                    removeSchoolByIdTask.AsTask);

            // then
            actualSchoolValidationException.Should()
                .BeEquivalentTo(expectedSchoolValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSchoolAsync(It.IsAny<School>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}