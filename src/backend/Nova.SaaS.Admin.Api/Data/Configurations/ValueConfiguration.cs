using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.SaaS.Admin.Api.Data.Models;

namespace Nova.SaaS.Admin.Api.Data.Configurations
{
    class ValueConfiguration : IEntityTypeConfiguration<Value>
    {
        public void Configure(EntityTypeBuilder<Value> builder)
        {
            builder.ToTable("Values");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.Property(x => x.Created)
              .IsRequired(true);
        }
    }
}