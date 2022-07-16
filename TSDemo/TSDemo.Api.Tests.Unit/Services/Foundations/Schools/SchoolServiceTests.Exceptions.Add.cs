using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using TSDemo.Api.Models.Schools;
using TSDemo.Api.Models.Schools.Exceptions;
using Xunit;

namespace TSDemo.Api.Tests.Unit.Services.Foundations.Schools
{
    public partial class SchoolServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            School someSchool = CreateRandomSchool();
            SqlException sqlException = GetSqlException();

            var failedSchoolStorageException =
                new FailedSchoolStorageException(sqlException);

            var expectedSchoolDependencyException =
                new SchoolDependencyException(failedSchoolStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<School> addSchoolTask =
                this.schoolService.AddSchoolAsync(someSchool);

            SchoolDependencyException actualSchoolDependencyException =
                await Assert.ThrowsAsync<SchoolDependencyException>(
                    addSchoolTask.AsTask);

            // then
            actualSchoolDependencyException.Should()
                .BeEquivalentTo(expectedSchoolDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSchoolAsync(It.IsAny<School>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSchoolDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfSchoolAlreadyExsitsAndLogItAsync()
        {
            // given
            School randomSchool = CreateRandomSchool();
            School alreadyExistsSchool = randomSchool;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsSchoolException =
                new AlreadyExistsSchoolException(duplicateKeyException);

            var expectedSchoolDependencyValidationException =
                new SchoolDependencyValidationException(alreadyExistsSchoolException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<School> addSchoolTask =
                this.schoolService.AddSchoolAsync(alreadyExistsSchool);

            // then
            SchoolDependencyValidationException actualSchoolDependencyValidationException =
                await Assert.ThrowsAsync<SchoolDependencyValidationException>(
                    addSchoolTask.AsTask);

            actualSchoolDependencyValidationException.Should()
                .BeEquivalentTo(expectedSchoolDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSchoolAsync(It.IsAny<School>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            School someSchool = CreateRandomSchool();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidSchoolReferenceException =
                new InvalidSchoolReferenceException(foreignKeyConstraintConflictException);

            var expectedSchoolValidationException =
                new SchoolDependencyValidationException(invalidSchoolReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<School> addSchoolTask =
                this.schoolService.AddSchoolAsync(someSchool);

            // then
            SchoolDependencyValidationException actualSchoolDependencyValidationException =
                await Assert.ThrowsAsync<SchoolDependencyValidationException>(
                    addSchoolTask.AsTask);

            actualSchoolDependencyValidationException.Should().BeEquivalentTo(expectedSchoolValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSchoolAsync(someSchool),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}