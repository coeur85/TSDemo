using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using TSDemo.Api.Models.Schools;
using Xunit;

namespace TSDemo.Api.Tests.Unit.Services.Foundations.Schools
{
    public partial class SchoolServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSchoolByIdAsync()
        {
            // given
            School randomSchool = CreateRandomSchool();
            School inputSchool = randomSchool;
            School storageSchool = randomSchool;
            School expectedSchool = storageSchool.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSchoolByIdAsync(inputSchool.Id))
                    .ReturnsAsync(storageSchool);

            // when
            School actualSchool =
                await this.schoolService.RetrieveSchoolByIdAsync(inputSchool.Id);

            // then
            actualSchool.Should().BeEquivalentTo(expectedSchool);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(inputSchool.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}