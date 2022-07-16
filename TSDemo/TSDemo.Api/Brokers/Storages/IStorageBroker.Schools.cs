using System;
using System.Linq;
using System.Threading.Tasks;
using TSDemo.Api.Models.Schools;

namespace TSDemo.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<School> InsertSchoolAsync(School school);
    }
}
