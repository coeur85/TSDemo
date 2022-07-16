using System.Threading.Tasks;
using TSDemo.Api.Models.Schools;

namespace TSDemo.Api.Services.Foundations.Schools
{
    public interface ISchoolService
    {
        ValueTask<School> AddSchoolAsync(School school);
    }
}