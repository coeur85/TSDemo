using System;
using System.Linq;
using System.Threading.Tasks;
using TSDemo.Api.Models.Schools;

namespace TSDemo.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<School> InsertSchoolAsync(School school);
        IQueryable<School> SelectAllSchools();
        ValueTask<School> SelectSchoolByIdAsync(Guid schoolId);
        ValueTask<School> UpdateSchoolAsync(School school);
        ValueTask<School> DeleteSchoolAsync(School school);
    }
}
