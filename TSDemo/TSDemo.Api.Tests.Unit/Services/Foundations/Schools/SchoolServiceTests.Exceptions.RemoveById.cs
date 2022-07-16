using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            School randomSchool = CreateRandomSchool();
            SqlException sqlException = GetSqlException();

            var failedSchoolStorageException =
                new FailedSchoolStorageException(sqlException);

            var expectedSchoolDependencyException =
                new SchoolDependencyException(failedSchoolStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSchoolByIdAsync(randomSchool.Id))
                    .Throws(sqlException);

            // when
            ValueTask<School> addSchoolTask =
                this.schoolService.RemoveSchoolByIdAsync(randomSchool.Id);

            SchoolDependencyException actualSchoolDependencyException =
                await Assert.ThrowsAsync<SchoolDependencyException>(
                    addSchoolTask.AsTask);

            // then
            actualSchoolDependencyException.Should()
                .BeEquivalentTo(expectedSchoolDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(randomSchool.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSchoolDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSchoolAsync(It.IsAny<School>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someSchoolId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedSchoolException =
                new LockedSchoolException(databaseUpdateConcurrencyException);

            var expectedSchoolDependencyValidationException =
                new SchoolDependencyValidationException(lockedSchoolException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<School> removeSchoolByIdTask =
                this.schoolService.RemoveSchoolByIdAsync(someSchoolId);

            SchoolDependencyValidationException actualSchoolDependencyValidationException =
                await Assert.ThrowsAsync<SchoolDependencyValidationException>(
                    removeSchoolByIdTask.AsTask);

            // then
            actualSchoolDependencyValidationException.Should()
                .BeEquivalentTo(expectedSchoolDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSchoolAsync(It.IsAny<School>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someSchoolId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedSchoolStorageException =
                new FailedSchoolStorageException(sqlException);

            var expectedSchoolDependencyException =
                new SchoolDependencyException(failedSchoolStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<School> deleteSchoolTask =
                this.schoolService.RemoveSchoolByIdAsync(someSchoolId);

            SchoolDependencyException actualSchoolDependencyException =
                await Assert.ThrowsAsync<SchoolDependencyException>(
                    deleteSchoolTask.AsTask);

            // then
            actualSchoolDependencyException.Should()
                .BeEquivalentTo(expectedSchoolDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSchoolDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someSchoolId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedSchoolServiceException =
                new FailedSchoolServiceException(serviceException);

            var expectedSchoolServiceException =
                new SchoolServiceException(failedSchoolServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<School> removeSchoolByIdTask =
                this.schoolService.RemoveSchoolByIdAsync(someSchoolId);

            SchoolServiceException actualSchoolServiceException =
                await Assert.ThrowsAsync<SchoolServiceException>(
                    removeSchoolByIdTask.AsTask);

            // then
            actualSchoolServiceException.Should()
                .BeEquivalentTo(expectedSchoolServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}