using System.Threading.Tasks;
using TSDemo.Api.Models.Students;

namespace TSDemo.Api.Services.Foundations.Students
{
    public interface IStudentService
    {
        ValueTask<Student> AddStudentAsync(Student student);
    }
}