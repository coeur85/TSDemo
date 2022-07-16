using System;
using Moq;
using TSDemo.Api.Brokers.DateTimes;
using TSDemo.Api.Brokers.Loggings;
using TSDemo.Api.Brokers.Storages;
using TSDemo.Api.Models.Schools;
using TSDemo.Api.Services.Foundations.Schools;
using Tynamix.ObjectFiller;

namespace TSDemo.Api.Tests.Unit.Services.Foundations.Schools
{
    public partial class SchoolServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISchoolService schoolService;

        public SchoolServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.schoolService = new SchoolService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static School CreateRandomSchool(DateTimeOffset dateTimeOffset) =>
            CreateSchoolFiller(dateTimeOffset).Create();

        private static Filler<School> CreateSchoolFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<School>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(school => school.CreatedByUserId).Use(userId)
                .OnProperty(school => school.UpdatedByUserId).Use(userId);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}