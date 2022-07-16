using System;
using Moq;
using TSDemo.Api.Brokers.DateTimes;
using TSDemo.Api.Brokers.Loggings;
using TSDemo.Api.Brokers.Storages;
using TSDemo.Api.Models.Students;
using TSDemo.Api.Services.Foundations.Students;
using Tynamix.ObjectFiller;

namespace TSDemo.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IStudentService studentService;

        public StudentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.studentService = new StudentService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Student CreateRandomStudent(DateTimeOffset dateTimeOffset) =>
            CreateStudentFiller(dateTimeOffset).Create();

        private static Filler<Student> CreateStudentFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<Student>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(student => student.CreatedByUserId).Use(userId)
                .OnProperty(student => student.UpdatedByUserId).Use(userId);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}