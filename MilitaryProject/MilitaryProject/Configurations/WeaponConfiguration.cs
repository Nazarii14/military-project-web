using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryProject.Models;

namespace MilitaryProject.Configurations
{
    public class WeaponConfiguration : IEntityTypeConfiguration<Weapon>
    {
        public void Configure(EntityTypeBuilder<Weapon> builder)
        {
            builder.HasKey(w => w.ID);
            builder.Property(w => w.Type).IsRequired().HasMaxLength(50);
            builder.Property(w => w.Name).IsRequired().HasMaxLength(100);
            builder.Property(w => w.Price).IsRequired().HasColumnType("decimal(18, 2)");
            builder.Property(w => w.Weight).IsRequired().HasColumnType("decimal(18, 2)");

            builder.ToTable("Weapons");
        }
    }
}
