using System.Linq;
using FluentAssertions;
using Moq;
using TSDemo.Api.Models.Schools;
using Xunit;

namespace TSDemo.Api.Tests.Unit.Services.Foundations.Schools
{
    public partial class SchoolServiceTests
    {
        [Fact]
        public void ShouldReturnSchools()
        {
            // given
            IQueryable<School> randomSchools = CreateRandomSchools();
            IQueryable<School> storageSchools = randomSchools;
            IQueryable<School> expectedSchools = storageSchools;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSchools())
                    .Returns(storageSchools);

            // when
            IQueryable<School> actualSchools =
                this.schoolService.RetrieveAllSchools();

            // then
            actualSchools.Should().BeEquivalentTo(expectedSchools);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSchools(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}