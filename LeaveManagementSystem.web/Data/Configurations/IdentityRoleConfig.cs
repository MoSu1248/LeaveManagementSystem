using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.web.Data.Configurations
{
    public class IdentityRoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
           builder.HasData(new IdentityRole
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
        }
    }
}
