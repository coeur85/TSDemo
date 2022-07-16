using System;
using System.Linq;
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

        public ValueTask<School> AddSchoolAsync(School school) =>
            TryCatch(async () =>
            {
                ValidateSchoolOnAdd(school);

                return await this.storageBroker.InsertSchoolAsync(school);
            });

        public IQueryable<School> RetrieveAllSchools() =>
            TryCatch(() => this.storageBroker.SelectAllSchools());

        public ValueTask<School> RetrieveSchoolByIdAsync(Guid schoolId) =>
            TryCatch(async () =>
            {
                ValidateSchoolId(schoolId);

                School maybeSchool = await this.storageBroker
                    .SelectSchoolByIdAsync(schoolId);

                ValidateStorageSchool(maybeSchool, schoolId);

                return maybeSchool;
            });

        public ValueTask<School> ModifySchoolAsync(School school) =>
            throw new NotImplementedException();
    }
}