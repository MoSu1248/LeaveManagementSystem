using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LeaveType> LeaveTypes { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "74a5d287-a501-4980-9d36-76b329b50b52",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = "11111111-1111-1111-1111-111111111111"
                },
                new IdentityRole
                {
                    Id = "24da7188-56a9-4bbc-84d3-7fe7d9a9d629",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR",
                    ConcurrencyStamp = "22222222-2222-2222-2222-222222222222"
                },
                new IdentityRole
                {
                    Id = "73dda014-59df-4122-b450-dd8b2c50f034",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "33333333-3333-3333-3333-333333333333"
                }
            );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "09b9dd78-1e2a-4873-beff-e0904bd81334",
                    UserName = "admin@localhost.com",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    EmailConfirmed = true,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateOnly(1980, 1, 1),
                    // Replace this with a hash you generate locally if desired.
                    PasswordHash = "P@ssword123",
                    SecurityStamp = "f1420e0d-750e-4a1b-9c2d-0123456789ab",
                    ConcurrencyStamp = "44444444-4444-4444-4444-444444444444"
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "74a5d287-a501-4980-9d36-76b329b50b52",
                    UserId = "09b9dd78-1e2a-4873-beff-e0904bd81334"
                }
            );
        }
    }
}