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
        public async Task ShouldThrowValidationExceptionOnAddIfSchoolIsNullAndLogItAsync()
        {
            // given
            School nullSchool = null;

            var nullSchoolException =
                new NullSchoolException();

            var expectedSchoolValidationException =
                new SchoolValidationException(nullSchoolException);

            // when
            ValueTask<School> addSchoolTask =
                this.schoolService.AddSchoolAsync(nullSchool);

            SchoolValidationException actualSchoolValidationException =
                await Assert.ThrowsAsync<SchoolValidationException>(
                    addSchoolTask.AsTask);

            // then
            actualSchoolValidationException.Should().BeEquivalentTo(expectedSchoolValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSchoolValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}