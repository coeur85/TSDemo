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
        public async Task ShouldModifySchoolAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            School randomSchool = CreateRandomModifySchool(randomDateTimeOffset);
            School inputSchool = randomSchool;
            School storageSchool = inputSchool.DeepClone();
            storageSchool.UpdatedDate = randomSchool.CreatedDate;
            School updatedSchool = inputSchool;
            School expectedSchool = updatedSchool.DeepClone();
            Guid schoolId = inputSchool.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSchoolByIdAsync(schoolId))
                    .ReturnsAsync(storageSchool);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSchoolAsync(inputSchool))
                    .ReturnsAsync(updatedSchool);

            // when
            School actualSchool =
                await this.schoolService.ModifySchoolAsync(inputSchool);

            // then
            actualSchool.Should().BeEquivalentTo(expectedSchool);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSchoolByIdAsync(inputSchool.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSchoolAsync(inputSchool),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}