using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace LeaveManagementSystem.web.Data.Configurations
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(
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

        }
    }
}
