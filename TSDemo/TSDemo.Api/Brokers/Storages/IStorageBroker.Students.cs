using System;
using System.Linq;
using System.Threading.Tasks;
using TSDemo.Api.Models.Students;

namespace TSDemo.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Student> InsertStudentAsync(Student student);
        IQueryable<Student> SelectAllStudents();
        ValueTask<Student> SelectStudentByIdAsync(Guid studentId);
    }
}
