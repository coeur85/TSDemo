using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using TSDemo.Api.Models.Schools;
using TSDemo.Api.Models.Schools.Exceptions;
using Xunit;

namespace TSDemo.Api.Tests.Unit.Services.Foundations.Schools
{
    public partial class SchoolServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            School randomSchool = CreateRandomSchool();
            SqlException sqlException = GetSqlException();

            var failedSchoolStorageException =
                new FailedSchoolStorageException(sqlException);

            var expectedSchoolDependencyException =
                new SchoolDependencyException(failedSchoolStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<School> modifySchoolTask =
                this.schoolService.ModifySchoolAsync(randomSchool);

            SchoolDependencyException actualSchoolDependencyException =
                await Assert.ThrowsAsync<SchoolDependencyException>(
                    modifySchoolTask.AsTask);

            // then
            actualSchoolDependencyException.Should()
                .BeEquivalentTo(expectedSchoolDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(randomSchool.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSchoolDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSchoolAsync(randomSchool),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            School someSchool = CreateRandomSchool();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidSchoolReferenceException =
                new InvalidSchoolReferenceException(foreignKeyConstraintConflictException);

            SchoolDependencyValidationException expectedSchoolDependencyValidationException =
                new SchoolDependencyValidationException(invalidSchoolReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<School> modifySchoolTask =
                this.schoolService.ModifySchoolAsync(someSchool);

            SchoolDependencyValidationException actualSchoolDependencyValidationException =
                await Assert.ThrowsAsync<SchoolDependencyValidationException>(
                    modifySchoolTask.AsTask);

            // then
            actualSchoolDependencyValidationException.Should()
                .BeEquivalentTo(expectedSchoolDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(someSchool.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSchoolDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSchoolAsync(someSchool),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            School randomSchool = CreateRandomSchool();
            var databaseUpdateException = new DbUpdateException();

            var failedSchoolStorageException =
                new FailedSchoolStorageException(databaseUpdateException);

            var expectedSchoolDependencyException =
                new SchoolDependencyException(failedSchoolStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<School> modifySchoolTask =
                this.schoolService.ModifySchoolAsync(randomSchool);

            SchoolDependencyException actualSchoolDependencyException =
                await Assert.ThrowsAsync<SchoolDependencyException>(
                    modifySchoolTask.AsTask);

            // then
            actualSchoolDependencyException.Should()
                .BeEquivalentTo(expectedSchoolDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(randomSchool.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSchoolAsync(randomSchool),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            School randomSchool = CreateRandomSchool();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedSchoolException =
                new LockedSchoolException(databaseUpdateConcurrencyException);

            var expectedSchoolDependencyValidationException =
                new SchoolDependencyValidationException(lockedSchoolException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<School> modifySchoolTask =
                this.schoolService.ModifySchoolAsync(randomSchool);

            SchoolDependencyValidationException actualSchoolDependencyValidationException =
                await Assert.ThrowsAsync<SchoolDependencyValidationException>(
                    modifySchoolTask.AsTask);

            // then
            actualSchoolDependencyValidationException.Should()
                .BeEquivalentTo(expectedSchoolDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(randomSchool.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSchoolAsync(randomSchool),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            School randomSchool = CreateRandomSchool();
            var serviceException = new Exception();

            var failedSchoolServiceException =
                new FailedSchoolServiceException(serviceException);

            var expectedSchoolServiceException =
                new SchoolServiceException(failedSchoolServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<School> modifySchoolTask =
                this.schoolService.ModifySchoolAsync(randomSchool);

            SchoolServiceException actualSchoolServiceException =
                await Assert.ThrowsAsync<SchoolServiceException>(
                    modifySchoolTask.AsTask);

            // then
            actualSchoolServiceException.Should()
                .BeEquivalentTo(expectedSchoolServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(randomSchool.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSchoolAsync(randomSchool),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}