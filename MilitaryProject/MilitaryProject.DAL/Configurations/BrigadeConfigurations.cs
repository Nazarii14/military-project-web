using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryProject.Domain.Entity;

namespace MilitaryProject.DAL.Configurations
{
    public class BrigadeConfiguration : IEntityTypeConfiguration<Brigade>
    {
        public void Configure(EntityTypeBuilder<Brigade> builder)
        {
            builder.HasKey(b => b.ID);
            builder.Property(b => b.Name).IsRequired().HasMaxLength(200);
            builder.Property(b => b.CommanderName).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Location).IsRequired().HasMaxLength(200);

            builder.ToTable("Brigades");
        }
    }
}