using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryProject.Models;

namespace MilitaryProject.Configurations
{
    public class AmmunitionConfiguration : IEntityTypeConfiguration<Ammunition>
    {
        public void Configure(EntityTypeBuilder<Ammunition> builder)
        {
            builder.HasKey(a => a.ID);
            builder.Property(a => a.Type).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Price).IsRequired().HasColumnType("decimal(18, 2)");
            builder.Property(a => a.Size).IsRequired().HasMaxLength(50);

            builder.ToTable("Ammunitions");
        }
    }
}
