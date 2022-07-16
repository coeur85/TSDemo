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
    }
}
