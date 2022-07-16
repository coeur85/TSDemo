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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidSchoolId = Guid.Empty;

            var invalidSchoolException =
                new InvalidSchoolException();

            invalidSchoolException.AddData(
                key: nameof(School.Id),
                values: "Id is required");

            var expectedSchoolValidationException =
                new SchoolValidationException(invalidSchoolException);

            // when
            ValueTask<School> retrieveSchoolByIdTask =
                this.schoolService.RetrieveSchoolByIdAsync(invalidSchoolId);

            SchoolValidationException actualSchoolValidationException =
                await Assert.ThrowsAsync<SchoolValidationException>(
                    retrieveSchoolByIdTask.AsTask);

            // then
            actualSchoolValidationException.Should()
                .BeEquivalentTo(expectedSchoolValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfSchoolIsNotFoundAndLogItAsync()
        {
            //given
            Guid someSchoolId = Guid.NewGuid();
            School noSchool = null;

            var notFoundSchoolException =
                new NotFoundSchoolException(someSchoolId);

            var expectedSchoolValidationException =
                new SchoolValidationException(notFoundSchoolException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noSchool);

            //when
            ValueTask<School> retrieveSchoolByIdTask =
                this.schoolService.RetrieveSchoolByIdAsync(someSchoolId);

            SchoolValidationException actualSchoolValidationException =
                await Assert.ThrowsAsync<SchoolValidationException>(
                    retrieveSchoolByIdTask.AsTask);

            //then
            actualSchoolValidationException.Should().BeEquivalentTo(expectedSchoolValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}