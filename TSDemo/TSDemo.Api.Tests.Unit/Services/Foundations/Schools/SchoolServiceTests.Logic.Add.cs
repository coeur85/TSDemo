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

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSchoolAsync(inputSchool))
                    .ReturnsAsync(storageSchool);

            // when
            School actualSchool = await this.schoolService
                .AddSchoolAsync(inputSchool);

            // then
            actualSchool.Should().BeEquivalentTo(expectedSchool);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSchoolAsync(inputSchool),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}