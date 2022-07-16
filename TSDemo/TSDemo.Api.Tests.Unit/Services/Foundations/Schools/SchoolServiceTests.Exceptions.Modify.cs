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
    }
}