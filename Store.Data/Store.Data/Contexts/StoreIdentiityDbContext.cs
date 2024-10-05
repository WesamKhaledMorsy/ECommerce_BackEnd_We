using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Data.Entities.IdentityEntities;

namespace Store.Data.Contexts
{
    public class StoreIdentiityDbContext : IdentityDbContext<AppUser>
    {
        public StoreIdentiityDbContext(DbContextOptions<StoreIdentiityDbContext> options) : base(options)
        {

        }
        public DbSet<Address> Address { get; set; } 
    }
}
