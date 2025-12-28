using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class HealthRecordConfigurations : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
                .HasKey(X => X.Id);

            builder.HasOne<Member>()
                .WithOne(X => X.HealthRecord)
                .HasForeignKey<Member>(X => X.Id);

            builder.Property(H => H.Height)
                .HasPrecision(5, 2);

            builder.Property(H => H.Weight)
                .HasPrecision(5, 2);

            builder.Ignore(X => X.CreatedAt);

        }
    }
}
