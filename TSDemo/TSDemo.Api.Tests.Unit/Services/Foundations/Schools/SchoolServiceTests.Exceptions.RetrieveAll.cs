using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using TSDemo.Api.Models.Schools.Exceptions;
using Xunit;

namespace TSDemo.Api.Tests.Unit.Services.Foundations.Schools
{
    public partial class SchoolServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedSchoolStorageException(sqlException);

            var expectedSchoolDependencyException =
                new SchoolDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSchools())
                    .Throws(sqlException);

            // when
            Action retrieveAllSchoolsAction = () =>
                this.schoolService.RetrieveAllSchools();

            SchoolDependencyException actualSchoolDependencyException =
                Assert.Throws<SchoolDependencyException>(retrieveAllSchoolsAction);

            // then
            actualSchoolDependencyException.Should()
                .BeEquivalentTo(expectedSchoolDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSchools(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedSchoolServiceException =
                new FailedSchoolServiceException(serviceException);

            var expectedSchoolServiceException =
                new SchoolServiceException(failedSchoolServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSchools())
                    .Throws(serviceException);

            // when
            Action retrieveAllSchoolsAction = () =>
                this.schoolService.RetrieveAllSchools();

            SchoolServiceException actualSchoolServiceException =
                Assert.Throws<SchoolServiceException>(retrieveAllSchoolsAction);

            // then
            actualSchoolServiceException.Should()
                .BeEquivalentTo(expectedSchoolServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSchools(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}