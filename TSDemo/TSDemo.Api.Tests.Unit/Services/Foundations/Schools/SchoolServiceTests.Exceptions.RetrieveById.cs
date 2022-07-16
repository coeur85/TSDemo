using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedSchoolStorageException =
                new FailedSchoolStorageException(sqlException);

            var expectedSchoolDependencyException =
                new SchoolDependencyException(failedSchoolStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSchoolByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<School> retrieveSchoolByIdTask =
                this.schoolService.RetrieveSchoolByIdAsync(someId);

            SchoolDependencyException actualSchoolDependencyException =
                await Assert.ThrowsAsync<SchoolDependencyException>(
                    retrieveSchoolByIdTask.AsTask);

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
    }
}