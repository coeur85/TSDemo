using System.Threading.Tasks;
using TSDemo.Api.Brokers.DateTimes;
using TSDemo.Api.Brokers.Loggings;
using TSDemo.Api.Brokers.Storages;
using TSDemo.Api.Models.Schools;

namespace TSDemo.Api.Services.Foundations.Schools
{
    public partial class SchoolService : ISchoolService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public SchoolService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<School> AddSchoolAsync(School school) =>
            await this.storageBroker.InsertSchoolAsync(school);
    }
}