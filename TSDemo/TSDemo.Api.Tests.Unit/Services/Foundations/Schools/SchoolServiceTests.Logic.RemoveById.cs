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
        public async Task ShouldRemoveSchoolByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputSchoolId = randomId;
            School randomSchool = CreateRandomSchool();
            School storageSchool = randomSchool;
            School expectedInputSchool = storageSchool;
            School deletedSchool = expectedInputSchool;
            School expectedSchool = deletedSchool.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSchoolByIdAsync(inputSchoolId))
                    .ReturnsAsync(storageSchool);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSchoolAsync(expectedInputSchool))
                    .ReturnsAsync(deletedSchool);

            // when
            School actualSchool = await this.schoolService
                .RemoveSchoolByIdAsync(inputSchoolId);

            // then
            actualSchool.Should().BeEquivalentTo(expectedSchool);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(inputSchoolId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSchoolAsync(expectedInputSchool),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}