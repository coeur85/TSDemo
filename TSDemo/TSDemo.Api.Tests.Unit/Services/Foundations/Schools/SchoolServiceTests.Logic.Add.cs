using System;
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
        public async Task ShouldAddSchoolAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            School randomSchool = CreateRandomSchool(randomDateTimeOffset);
            School inputSchool = randomSchool;
            School storageSchool = inputSchool;
            School expectedSchool = storageSchool.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSchoolAsync(inputSchool))
                    .ReturnsAsync(storageSchool);

            // when
            School actualSchool = await this.schoolService
                .AddSchoolAsync(inputSchool);

            // then
            actualSchool.Should().BeEquivalentTo(expectedSchool);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSchoolAsync(inputSchool),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}