using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TSDemo.Api.Models.Schools;

namespace TSDemo.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<School> Schools { get; set; }

        public async ValueTask<School> InsertSchoolAsync(School school)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<School> schoolEntityEntry =
                await broker.Schools.AddAsync(school);

            await broker.SaveChangesAsync();

            return schoolEntityEntry.Entity;
        }

        public IQueryable<School> SelectAllSchools()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Schools;
        }

        public async ValueTask<School> SelectSchoolByIdAsync(Guid schoolId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.Schools.FindAsync(schoolId);
        }

        public async ValueTask<School> UpdateSchoolAsync(School school)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<School> schoolEntityEntry =
                broker.Schools.Update(school);

            await broker.SaveChangesAsync();

            return schoolEntityEntry.Entity;
        }

        public async ValueTask<School> DeleteSchoolAsync(School school)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<School> schoolEntityEntry =
                broker.Schools.Remove(school);

            await broker.SaveChangesAsync();

            return schoolEntityEntry.Entity;
        }
    }
}
