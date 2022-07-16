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
    }
}