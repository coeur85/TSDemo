using TSDemo.Api.Models.Schools;
using TSDemo.Api.Models.Schools.Exceptions;

namespace TSDemo.Api.Services.Foundations.Schools
{
    public partial class SchoolService
    {
        private void ValidateSchoolOnAdd(School school)
        {
            ValidateSchoolIsNotNull(school);
        }

        private static void ValidateSchoolIsNotNull(School school)
        {
            if (school is null)
            {
                throw new NullSchoolException();
            }
        }
    }
}