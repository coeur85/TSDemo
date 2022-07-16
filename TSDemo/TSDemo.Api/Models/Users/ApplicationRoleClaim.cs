using Microsoft.AspNetCore.Identity;
using System;

namespace TSDemo.Api.Models.Users
{
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
